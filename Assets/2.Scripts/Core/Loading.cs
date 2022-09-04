using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class Loading : MonoBehaviour
{
    // Start is called before the first frame update
    private IEnumerator Start()
    {
        Settings.SaveSettings();
        yield return Resources.UnloadUnusedAssets();
        GC.Collect();

        //得到小剧场
        var all = YamlReadWrite.ReadDialogues();
        int i = UnityEngine.Random.Range(0, all.Length);
        //清单文件获取
        Settings.selectedDialogue = YamlReadWrite.ReadDialogues()[i];
        //得到图片
        UnityWebRequest d =
            new UnityWebRequest(
                $"file://{YamlReadWrite.UnityButNotAssets}/Dialogue/{Settings.selectedDialogue.BackgroundImageName}.jpg");
        DownloadHandlerTexture downloadHandlerTexture = new DownloadHandlerTexture(true);
        d.downloadHandler = downloadHandlerTexture;
        yield return d.SendWebRequest();

        Texture2D texture = downloadHandlerTexture.texture;
        Settings.dialogueImage = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero);

        yield return SceneManager.LoadSceneAsync("SampleScene");
    }
}