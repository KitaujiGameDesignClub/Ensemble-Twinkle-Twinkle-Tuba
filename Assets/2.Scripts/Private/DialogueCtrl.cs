using System;
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

public AudioClip bgm;




[ContextMenu("测试小剧场")]
  public void Start()
  {
//清单文件获取
    var  selectedDialogue = Loading.selectedDialogue;
    //得到图片
    var image = Loading.dialogueImage;
    

    BG.sprite = image;
    
    //播放bgm
    PublicAudioSource.publicAudioSource.PlayBackgroundMusic(bgm);
    
    //读取内容
    //读取内容
  
    if (selectedDialogue.extraContent != "可选内容​" && selectedDialogue.extraContent != "可选内容")
    {
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
