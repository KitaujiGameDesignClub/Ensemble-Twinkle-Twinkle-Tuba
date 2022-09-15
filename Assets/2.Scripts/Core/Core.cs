using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

/// <summary>
/// 负责场景切换与各类必要变量的储存
/// </summary>
public class Core : MonoBehaviour,IUpdate
{
    public static Core core;
    
    /// <summary>
    /// 选择的乐器编号
    /// </summary>
    public int selectedInstrument;

    /// <summary>
    /// 到哪一章节了
    /// </summary>
    public int episode = 0;
    


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
        Key12Space,
        Key13Space,
        Key1Space,
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

public GameObject AndroidScreenButton;

    private void Awake()
    {
        core = this;
        
        //不选择乐器
        selectedInstrument = -1;
        //禁用游戏本体
        gameSelf.SetActive(false);
     
        //打开选择角色的面板
        chooseCharacter.SetActive(true);
       

        episode = 0;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
      Destroy(AndroidScreenButton);
      
      #elif UNITY_ANDROID
      AndroidScreenButton.SetActive(true);
#endif

    }

    private void Start()
    {
        UpdateManager.RegisterUpdate(this);
  
    }


    public void StartGame()
    {
        if (selectedInstrument < 0)
        {
            return;
        }
        
        //停止BGM继续播放
        PublicAudioSource.publicAudioSource.StopMusicPlaying();
        //开始游戏
        DemonstrateInstrument();
        //开始游戏
        PublicAudioSource.publicAudioSource.PlaySoundEffect(PublicAudioSource.AudioType.Click);
     
        
    }

    /// <summary>
    /// 现在是开始游戏了
    /// </summary>
    /// <param name="id"></param>
    private void DemonstrateInstrument()
    {
      
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
      
        dialogue.SetActive(true);
        //摧毁游戏面板
        Destroy(gameSelf);
    }
    
    // Start is called before the first frame update

    public void FastUpdate()
    {
        //重新开始游戏
        if (Input.GetKeyDown(KeyCode.R))
        {
            Retry();
        }
        //暂停游戏
        if (Input.GetKeyDown(KeyCode.Escape))
        {
           
          Pause();
           
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene("SampleScene");
    }



    public void Pause()
    {
        if (Time.timeScale >= 0.5F)
        {
            Time.timeScale = 0f;
            StaticVideoPlayer.staticVideoPlayer.Pause();
        }
        else
        {
            Time.timeScale = 1f;
            StaticVideoPlayer.staticVideoPlayer.Play();
        }
    }

    public void skip()
    {
        //跳过前面的片段
        if (episode == 0)
        {
            StaticVideoPlayer.staticVideoPlayer.Jump(Episode.ShowStaffAndInstrument -1);
        }
        
        //跳过后面的片段
        if (episode == 3)
        {
            StaticVideoPlayer.staticVideoPlayer.Jump(Episode.VideoEnd - 1);
        }
    }
    
}
