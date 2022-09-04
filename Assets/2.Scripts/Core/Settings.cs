using UnityEngine;

public class Settings
{
    public static YamlReadWrite.SettingsContent SettingsContent;
    /// <summary>
    /// 本轮游戏的小剧场
    /// </summary>
    public static YamlReadWrite.Dialogue selectedDialogue;
/// <summary>
/// 本轮游戏用的小剧场的背景图片
/// </summary>
    public static Sprite dialogueImage;

    /// <summary>
    /// 读取设置文件
    /// </summary>
    public static void ReadSettings()
    {
        SettingsContent = YamlReadWrite.Read<YamlReadWrite.SettingsContent>(YamlReadWrite.FileName.Settings);
        
        
    }

    /// <summary>
    /// 保存设置至文件
    /// </summary>
    public static void SaveSettings()
    {
        YamlReadWrite.Write(SettingsContent,YamlReadWrite.FileName.Settings,"#游戏设置");
    }

}
