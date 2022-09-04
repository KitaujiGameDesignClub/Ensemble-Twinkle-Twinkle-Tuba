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
        Settings,
        PsitonsAction,
        Cymbal,
        Dialogue,
    }


    /// <summary>
    /// Assets上一级的目录（结尾没有/）
    /// </summary>
    /// <returns></returns>
    public static string UnityButNotAssets
    {
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
        
        
//检查是否存在所需的游戏文件夹，不存在则创建
        CheckAndCreateDirectory();
        Serializer serializer = new Serializer();

        //把注释写入的内容
        string authenticContent = $"{notes}\n{serializer.Serialize(content)}";

       
            StreamWriter  streamWriter =
                new StreamWriter($"{UnityButNotAssets}/saves/{fileName.ToString()}.yaml", false, Encoding.UTF8);
  
       
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
      
            //检查是否存在所需的游戏文件夹，不存在则创建
            CheckAndCreateDirectory();
            Deserializer deserializer = new();
      
      
            StreamReader streamReader =
                new StreamReader($"{UnityButNotAssets}/saves/{fileName.ToString()}.yaml", Encoding.UTF8);
        
  

            var content = deserializer.Deserialize<T>(streamReader.ReadToEnd());
            streamReader.Dispose();
            streamReader.Close();
            return content;
    }

    /// <summary>
    /// 读取小剧场的清单文件
    /// </summary>
    /// <returns></returns>
    public static Dialogue[] ReadDialogues()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo($"{UnityButNotAssets}/Dialogue");

       var manifests = directoryInfo.GetFiles("*.yaml");

       Dialogue[] dialogues = new Dialogue[manifests.Length];
       Deserializer deserializer = new Deserializer();

       for (int i = 0; i < dialogues.Length; i++)
       {
           FileStream fileStream = new FileStream(manifests[i].FullName, FileMode.Open, FileAccess.Read);
           StreamReader streamReader = new StreamReader(fileStream);
           //yaml读取
           var content = deserializer.Deserialize<Dialogue>(streamReader.ReadToEnd());
           streamReader.Dispose();
           streamReader.Close();
           dialogues[i] = content;
           
          
       }

       return dialogues;




    }


    /// <summary>
    /// 检查是否存在所需的游戏文件夹，不存在则创建
    /// </summary>
    public static void CheckAndCreateDirectory()
    {
        if (!Directory.Exists($"{UnityButNotAssets}/saves"))
        {
            Directory.CreateDirectory($"{UnityButNotAssets}/saves");
        }
    }


    #region yaml用的各种结构体（类）
      
    /// <summary>
    /// 储存大镲时间的结构体
    /// </summary>
    [Serializable]
    public struct CymbalAction
    {
        public string[] time;
    }

    /// <summary>
    /// 设置的内容
    /// </summary>
    [Serializable]
    public struct SettingsContent
    {
        public float MusicVolume;
        public float SoundEffectVolume;

        public int lag;

        //已经玩完这个游戏了，便于开启“跳过：老师的话”功能
        public bool hasPlayed;
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