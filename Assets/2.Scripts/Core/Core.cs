using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    public static Core core;
    
    /// <summary>
    /// 选择的乐器编号
    /// </summary>
    public static int selectedInstrument;

    /// <summary>
    /// 到哪一章节了
    /// </summary>
    public static int episode = 0;

/// <summary>
/// 乐器指法
/// </summary>
    public enum Fingering
    {
        LeftC,
        LeftG,
        LeftD,
        A1,
        bE,
        bB1,
        d,
        A,
        be,
        bB,
        F,
        C,
        f,
        RightC,
        RIghtG,
        RightD,
        Key1,
        Key12,
        Key13,
        Space,
        Null
    }
    
    [Header("三个场景")]
    public GameObject chooseCharacter;
  /// <summary>
  /// 游戏本体
  /// </summary>
    public GameObject gameSelf;
/// <summary>
/// 游戏结束后的小剧场
/// </summary>
  public GameObject dialogue;


    private void Awake()
    {
        core = this;
        
        //不选择乐器
        selectedInstrument = -1;
        
        //打开选择角色的面板
        chooseCharacter.SetActive(true);
        //禁用游戏本体
        gameSelf.SetActive(false);

        episode = 0;

    }

    public void StartGame(int id)
    {
        //开始游戏
        DemonstrateInstrument(id);
    }

    /// <summary>
    /// 现在是开始游戏了
    /// </summary>
    /// <param name="id"></param>
    private void DemonstrateInstrument(int id)
    {
        selectedInstrument = id;
        //启用游戏本体
        gameSelf.SetActive(true);
        //摧毁选择角色的面板
        Destroy(chooseCharacter);
      
    }

    /// <summary>
    /// 显示小剧场
    /// </summary>
    public void ShowDialogue()
    {
        gameSelf.SetActive(false);
        dialogue.SetActive(true);
    }
    
    // Start is called before the first frame update

}
