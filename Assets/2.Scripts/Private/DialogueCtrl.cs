using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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
public TMP_Text Writer;
public Image BG;

/// <summary>
/// 本轮游戏的小剧场
/// </summary>
private YamlReadWrite.Dialogue selectedDialogue;

[ContextMenu("测试小剧场")]
  public void Show()
  {
#if UNITY_EDITOR
    StartCoroutine(loadDialogue());
#endif
    
    
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
     BG.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero);
    
    
    //读取内容
    //读取内容
  
    if (selectedDialogue.extraContent != "可选内容​")
    {
      Debug.Log("S");
      Extra.text = selectedDialogue.extraContent;
    }
    else
    {
      Extra.text = string.Empty;
    }
    LeftContent.text = selectedDialogue.LeftContent;
    RightContent.text = selectedDialogue.RightContent;
    Writer.text = selectedDialogue.Writer;
    characterIcons[0].GetSpriteFromAtlas($"Euphonium-characters_{selectedDialogue.characterLeft}");
    characterIcons[1].GetSpriteFromAtlas($"Euphonium-characters_{selectedDialogue.characterRight}");

  }
}
