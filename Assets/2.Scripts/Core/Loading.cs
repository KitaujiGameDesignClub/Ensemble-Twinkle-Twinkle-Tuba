using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class Loading : MonoBehaviour
{
    /// <summary>
    /// 本轮游戏的小剧场
    /// </summary>
  public static YamlReadWrite.Dialogue selectedDialogue;
    /// <summary>
    /// 本轮游戏的小剧场的背景图
    /// </summary>
     public   static Sprite dialogueImage;
    
    
    // Start is called before the first frame update
    private IEnumerator Start()
    {
        Settings.SaveSettings();
        yield return Resources.UnloadUnusedAssets();
        GC.Collect();
      //得到本轮的小剧场
        yield return loadDialogue();
    
        yield return SceneManager.LoadSceneAsync("SampleScene");
    }
    
    public IEnumerator loadDialogue()
    {
        var all = YamlReadWrite.ReadDialogues();
        int i = UnityEngine.Random.Range(0, all.Length);
        //清单文件获取
        selectedDialogue = YamlReadWrite.ReadDialogues()[i];
        //得到图片
        UnityWebRequest d =
            new UnityWebRequest(
                $"file://{System.IO.Path.GetDirectoryName(Application.dataPath)}/Dialogue/{selectedDialogue.BackgroundImageName}.jpg");
 
    
        DownloadHandlerTexture downloadHandlerTexture = new DownloadHandlerTexture(true);
        d.downloadHandler = downloadHandlerTexture;
        yield return d.SendWebRequest();

        Texture2D texture = downloadHandlerTexture.texture;
        dialogueImage = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero);
        

    }
}