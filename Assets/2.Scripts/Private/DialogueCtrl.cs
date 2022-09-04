using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class DialogueCtrl : MonoBehaviour
{
  /// <summary>
  /// 小剧场本体
  /// </summary>
  public GameObject dialogue;
/// <summary>
/// 0左 1 右
/// </summary>
  public AtlasRead[] characterIcons;

public TMP_Text Extra;
public TMP_Text LeftContent;
public TMP_Text RightContent;

[ContextMenu("测试小剧场")]
  public void Show()
  {
#if UNITY_EDITOR
    StartCoroutine(loadDialogue());
#endif
    
    
  }

  /// <summary>
  /// 修复头像的朝向
  /// </summary>
  private void fix()
  {
    
  }


  private IEnumerator loadDialogue()
  {
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
    
    
    fix();
  }
}
