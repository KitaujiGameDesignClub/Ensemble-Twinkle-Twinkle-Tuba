using System;
using System.IO;
using UnityEngine;

public class Settings
{
    public static YamlReadWrite.SettingsContent SettingsContent;
  


    /// <summary>
    /// 读取设置文件
    /// </summary>
    public static void ReadSettings()
    {
        string path = String.Empty;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        path = YamlReadWrite.UnityButNotAssets;
        
        #elif UNITY_ANDROID
          path = Application.persistentDataPath;
#endif
      

        //存在的话就读取
        if (File.Exists($"{path}/saves/Settings.yaml"))
        {
            SettingsContent = YamlReadWrite.Read<YamlReadWrite.SettingsContent>(YamlReadWrite.FileName.Settings);
        }
        //不存在的话，初始化一个
        else
        {
            SettingsContent = InitializeSettings();
            SaveSettings();
        }
     
        
        
    }

    /// <summary>
    /// 保存设置至文件
    /// </summary>
    public static void SaveSettings()
    {
        YamlReadWrite.Write(SettingsContent,YamlReadWrite.FileName.Settings,"#游戏设置");
    }

    public static YamlReadWrite.SettingsContent InitializeSettings()
    {
        var s = new YamlReadWrite.SettingsContent
        {
            MusicVolume = 1f,
            SoundEffectVolume = 1f
        };

        return s;
    }

}
