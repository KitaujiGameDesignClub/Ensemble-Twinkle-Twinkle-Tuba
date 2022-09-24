using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameInitialization : MonoBehaviour
{

#if UNITY_EDITOR
    public bool textGame = true;
#endif

    private void Awake()
    {
#if  !UNITY_ANDROID
        QualitySettings.vSyncCount = 1;
        //降低帧率 省点电
        OnDemandRendering.renderFrameInterval = 3;
        #else
           QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        //降低帧率 省点电
        OnDemandRendering.renderFrameInterval = 3;
#endif
        
        
     
    }

    private void Start()
    {
        
        //检查是否存在所需的游戏文件夹，不存在则创建
       YamlReadWrite.CheckAndCreateDirectory();
       //读取设置文件
       Settings.ReadSettings();
       //调整音量
       PublicAudioSource.publicAudioSource.UpdateMusicVolume();
       
       //加载场景
#if !UNITY_EDITOR
          SceneManager.LoadScene("Opening");
        
        #else
      
        SceneManager.LoadScene(textGame ? "load" : "Opening");
#endif
    }

   
}
