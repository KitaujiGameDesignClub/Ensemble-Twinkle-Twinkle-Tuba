using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitialization : MonoBehaviour
{
#if UNITY_EDITOR
    public bool textGame = true;
#endif
    
    private void Start()
    {
#if !UNITY_EDITOR
          SceneManager.LoadScene("Opening");
        
        #else
        //读取设置文件
        Settings.ReadSettings();
        //调整音量
        PublicAudioSource.publicAudioSource.UpdateMusicVolume();
        SceneManager.LoadScene(textGame ? "load" : "Opening");
#endif
    }
}
