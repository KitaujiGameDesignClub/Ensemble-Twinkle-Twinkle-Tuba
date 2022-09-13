using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using YamlDotNet.Serialization;

public static class YamlReadWrite
{
    public enum FileName
    {
       
        DoubleBass,
        Tuba,
        Eupho,
        Cymbal,
        Dialogue,
        Settings,
    }


    /// <summary>
    /// Assets上一级的目录（结尾没有/）
    /// </summary>
    /// <returns></returns>
    public static string UnityButNotAssets
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        get
        {
            string[] raw = Application.dataPath.Split("/");

            string done = string.Empty;
            for (int i = 1; i < raw.Length - 1; i++)
            {
                done = $"{done}/{raw[i]}";
            }

            return done;
        }
        
   
        
#endif
      
    }

    /// <summary>
    /// 在规定的文件夹中写yaml文件
    /// </summary>
    /// <param name="content">内容</param>
    /// <param name="fileName">文件名</param>
    /// <param name="notes">注释（按照yaml规范书写）</param>
    /// <typeparam name="T"></typeparam>
    public static void Write<T>(T content, FileName fileName, string notes = null)
    {
        if (fileName == FileName.Dialogue)
        {
            Debug.LogError("请使用工具进行保存小剧场的操作");
            return;
            
        }
        
        

        Serializer serializer = new Serializer();

        //把注释写入的内容
        string authenticContent = $"{notes}\n{serializer.Serialize(content)}";

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        StreamWriter streamWriter =
            new StreamWriter($"{UnityButNotAssets}/saves/{fileName.ToString()}.yaml", false, Encoding.UTF8);

#elif UNITY_ANDROID
           StreamWriter streamWriter =
            new StreamWriter($"{Application.persistentDataPath}/saves/{fileName.ToString()}.yaml", false,
                Encoding.UTF8);
#endif
        streamWriter.Write(authenticContent);
        streamWriter.Dispose();
        streamWriter.Close();
     



    }

    /// <summary>
    /// 读取yaml
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T Read<T>(FileName fileName)
    {
      
         
            Deserializer deserializer = new();
            
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        StreamReader streamReader =
            new StreamReader($"{UnityButNotAssets}/saves/{fileName.ToString()}.yaml", Encoding.UTF8);
        var content = deserializer.Deserialize<T>(streamReader.ReadToEnd());
        streamReader.Dispose();
        streamReader.Close();
       return content;
#elif UNITY_ANDROID
            if (fileName == FileName.Settings)
        {
            StreamReader streamReader =
                new StreamReader($"{Application.persistentDataPath}/saves/{fileName.ToString()}.yaml", Encoding.UTF8);
            var content = deserializer.Deserialize<T>(streamReader.ReadToEnd());
            streamReader.Dispose();
            streamReader.Close();
            return content;
        }
        //除了设置文件，其他的yaml都从res文件夹中读取
        else
        {
            return deserializer.Deserialize<T>(Resources.Load($"saves/{fileName.ToString()}").ToString());
        }

#endif

     
  

          
    }

    /// <summary>
    /// 读取小剧场的清单文件
    /// </summary>
    /// <returns></returns>
    public static Dialogue[] ReadDialogues()
    {
      
        
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        DirectoryInfo directoryInfo = new DirectoryInfo($"{UnityButNotAssets}/Dialogue");
        var manifests = directoryInfo.GetFiles("*.yaml");
   
        Dialogue[] dialogues = new Dialogue[manifests.Length];
        Deserializer deserializer = new Deserializer();

        for (int i = 0; i < dialogues.Length; i++)
        {
            //
            FileStream fileStream = new FileStream(manifests[i].FullName, FileMode.Open, FileAccess.Read);
            StreamReader streamReader = new StreamReader(fileStream);
            //yaml读取
            dialogues[i] = deserializer.Deserialize<Dialogue>(streamReader.ReadToEnd());
            streamReader.Dispose();
            streamReader.Close();
            
           
          
        }
#elif UNITY_ANDROID
            var manifests = Resources.LoadAll("Dialogue/yaml");
       Dialogue[] dialogues = new Dialogue[manifests.Length];
       Deserializer deserializer = new Deserializer();
       for (int i = 0; i < manifests.Length; i++)
       {
           dialogues[i] = deserializer.Deserialize<Dialogue>(manifests[i].ToString());
       }
#endif

    
      

      

     

       return dialogues;




    }


    /// <summary>
    /// 检查是否存在所需的游戏文件夹，不存在则创建
    /// </summary>
    public static void CheckAndCreateDirectory()
    {
#if UNITY_EDITOR || UNITY_EDITOR_WIN
        if (!Directory.Exists($"{UnityButNotAssets}/saves"))
        {
            Directory.CreateDirectory($"{UnityButNotAssets}/saves");
        }
        if (!Directory.Exists($"{UnityButNotAssets}/Dialogue"))
        {
            Directory.CreateDirectory($"{UnityButNotAssets}/Dialogue");
        }
        
#elif UNITY_ANDROID
//安卓这边不允许小剧场外部储存
          if (!Directory.Exists($"{Application.persistentDataPath}/saves"))
        {
            Directory.CreateDirectory($"{Application.persistentDataPath}/saves");
        }
#endif
       
       
    }


    /// <summary>
    /// 【PR标记点专用】将有好的时间线转化为电脑可以用的（视频帧数） 
    /// </summary>
    public static int ConvertFriendlyToReadable(int videoFps, string friendlyConent,int lag)
    {
        //00:00:00:00
        string[] fix = friendlyConent.Split(':');
        return int.Parse(fix[3]) + int.Parse(fix[2]) * videoFps + int.Parse(fix[1]) * 60 * videoFps + lag;
    }

    #region yaml用的各种结构体（类）
      
    /// <summary>
    /// 乐器指法时间点位
    /// </summary>
    [Serializable]
    public struct StaffTime
    {
        public int lag;
        public string[] time;
    }

    /// <summary>
    /// 设置的内容（唯一一个永远是外部储存的）
    /// </summary>
    [Serializable]
    public struct SettingsContent
    {
        public float MusicVolume;
        public float SoundEffectVolume;


    }

/// <summary>
/// 小剧场
/// </summary>
[Serializable]
public struct Dialogue
{
    public string Writer;
    public int characterLeft;
    public int characterRight;
    public string LeftContent;
    public string RightContent;
    public string extraContent;
    public string BackgroundImageName;
}

    public enum characterId
    {
        /// <summary>
        /// 黄前久美子
        /// </summary>
        Kumiko,

        /// <summary>
        /// 高坂丽奈
        /// </summary>
        Reina,

        /// <summary>
        /// 川島 緑輝
        /// </summary>
        Sapphire,

        /// <summary>
        /// 加藤叶月
        /// </summary>
        Hazuki,

        /// <summary>
        /// 冢本秀一
        /// </summary>
        Shūichi,

        /// <summary>
        /// 小笠原 晴香
        /// </summary>
        Haruka,

        /// <summary>
        /// 田中明日香
        /// </summary>
        Asuka,
        
        /// <summary>
        /// 吉川 優子
        /// </summary>
        Yūko,
        
        /// <summary>
        /// 中世古 香織
        /// </summary>
        Kaori,

      

        /// <summary>
        /// 中川 夏紀
        /// </summary>
        Natsuki,

        /// <summary>
        /// 鎧塚 みぞれ
        /// </summary>
        Mizore,

        /// <summary>
        /// 傘木 希美
        /// </summary>
        Nozomi,

        /// <summary>
        /// 長瀬 梨子
        /// </summary>
        Riko,

        /// <summary>
        /// 後藤 卓也
        /// </summary>
        Takuya,

        /// <summary>
        /// 斎藤 葵
        /// </summary>
        Aoi,

        /// <summary>
        /// 滝 昇
        /// </summary>
        Taki,

        /// <summary>
        /// 松本 美知恵（副顾问）
        /// </summary>
        Matsu,
        
        /// <summary>
        /// 黄前 麻美子
        /// </summary>
        Mamiko,

        /// <summary>
        /// 新山 聡美
        /// </summary>
        Niiyama,

        /// <summary>
        /// 橋本 真博
        /// </summary>
        Hashimoto,

    
     
    }

    #endregion
}