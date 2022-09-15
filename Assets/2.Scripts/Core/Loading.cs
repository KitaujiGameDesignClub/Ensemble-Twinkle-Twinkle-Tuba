using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class Loading : MonoBehaviour
{
    public TMP_Text loadingState;
    
    
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
        PublicAudioSource.publicAudioSource.StopMusicPlaying();
        
        
        loadingState.text = "少女调音中.....\n清除无用资源";
        Settings.SaveSettings();  
        GC.Collect();
        yield return Resources.UnloadUnusedAssets();
        GC.Collect();
        yield return Resources.UnloadUnusedAssets();
        
      //得到本轮的小剧场
      loadingState.text = "少女调音中.....\n小剧场加载";
        yield return loadDialogue();
        loadingState.text = "少女调音中.....\n载入游戏场景";
        yield return SceneManager.LoadSceneAsync("SampleScene");
        
    }
    
    public IEnumerator loadDialogue()
    {
        var all = YamlReadWrite.ReadDialogues();
        int i = UnityEngine.Random.Range(0, all.Length);
        //清单文件获取
        selectedDialogue = YamlReadWrite.ReadDialogues()[i];
       
        
     
        //得到图片
#if  UNITY_EDITOR || UNITY_STANDALONE_WIN
        UnityWebRequest d =
            new UnityWebRequest(
                $"file://{System.IO.Path.GetDirectoryName(Application.dataPath)}/Dialogue/{selectedDialogue.BackgroundImageName}.jpg");
 
    
        DownloadHandlerTexture downloadHandlerTexture = new DownloadHandlerTexture(true);
        d.downloadHandler = downloadHandlerTexture;
        yield return d.SendWebRequest();

        Texture2D texture = downloadHandlerTexture.texture;

        
        //android从resources文件中获取 
#elif UNITY_ANDROID
      var request= Resources.LoadAsync<Texture2D>($"Dialogue/Images/{selectedDialogue.BackgroundImageName}");
        yield return request;
        Texture2D texture = request.asset as Texture2D;

  #endif
      
        dialogueImage = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero);
        

    }
}