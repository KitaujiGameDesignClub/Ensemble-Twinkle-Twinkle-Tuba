using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using System.Windows.Forms;
using TMPro;
using YamlDotNet.Serialization;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Tools : MonoBehaviour
{
    public Image BG;

    /// <summary>
    /// 本轮游戏的小剧场
    /// </summary>
    private YamlReadWrite.Dialogue selectedDialogue;

    /// <summary>
    /// 不是默认，就是全名（即含路径）
    /// </summary>
    private string BGImageFullPath = "Default";

    private int leftCharacterId;
    private int rightCharacterId;

    [Header("写")]
    /// <summary>
    /// 左侧人物对话
    /// </summary>
    public TMP_Text left;

    /// <summary>
    /// 右侧人物对话
    /// </summary>
    public TMP_Text right;

    /// <summary>
    /// 额外内容
    /// </summary>
    public TMP_Text Extra;

    public TMP_Text Writer;

    [Header("读")] public TMP_InputField leftField;

    public TMP_InputField rightField;

    public TMP_InputField writerField;
    public TMP_InputField ExtraField;

    public AtlasRead leftCharacter;
    public AtlasRead rightCharacter;

    string defaultPath = "";
    private string selectDir;

    void Start()
    {
        defaultPath = YamlReadWrite.UnityButNotAssets;
        BGImageFullPath = "Default";
    }


    /// <summary>
    /// 保存小剧场
    /// </summary>
    public void SelectFolder()
    {
        try
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "小剧场清单文件|*.yaml";
            sfd.Title = "小剧场保存路径（一个yaml一个图片）";
            sfd.InitialDirectory = defaultPath;
            sfd.ValidateNames = true;
            sfd.AutoUpgradeEnabled = true;
            sfd.OverwritePrompt = true;
            sfd.AddExtension = true;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                var s = new YamlReadWrite.Dialogue
                {
                    characterLeft = leftCharacterId,
                    characterRight = rightCharacterId,
                    extraContent = Extra.text,
                    LeftContent = left.text,
                    RightContent = right.text,
                    //fileName：路径+文件都有了
                    BackgroundImageName =
                        $"BG of {sfd.FileName[(sfd.FileName.LastIndexOf("\\", StringComparison.Ordinal) + 1)..]}",
                    Writer = Writer.text
                };
                if (BGImageFullPath == "Default")
                {
                    s.BackgroundImageName = BGImageFullPath;
                }

                var serializer = new Serializer();
                var sd = serializer.Serialize(s);
          

                //写yaml清单文件

                StreamWriter streamWriter = new StreamWriter(sfd.FileName, false, Encoding.UTF8);

                streamWriter.Write(sd);
                streamWriter.Dispose();
                streamWriter.Close();


                //把图片复制过来
                if (BGImageFullPath != "Default")
                {
                    System.IO.File.Copy(BGImageFullPath,
                        $"{sfd.FileName.Substring(0, sfd.FileName.LastIndexOf("\\"))}/{s.BackgroundImageName}.jpg",
                        true);
                }
            }
        }
        catch (Exception e)
        {
            MessageBox.Show("保存错误：\n" + e.Message);
        }
    }

    /// <summary>
    /// 读取背景图
    /// </summary>
    public void SelectFile()
    {
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.InitialDirectory = defaultPath;
        ofd.Title = "选择16：9的背景图片";
        ofd.Filter = "可用压缩图片格式|*.jpg";

        if (ofd.ShowDialog() == DialogResult.OK)
        {
          


            StartCoroutine(LoadImage(ofd.FileName));
        }
    }

    /// <summary>
    /// （选择背景图后）从本地加载背景图
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private IEnumerator LoadImage(string path)
    {  
        BGImageFullPath = path;
        UnityWebRequest d = new UnityWebRequest($"file://{path}");
   
        DownloadHandlerTexture downloadHandlerTexture = new DownloadHandlerTexture(true);
        d.downloadHandler = downloadHandlerTexture;
        yield return d.SendWebRequest();

        Texture2D texture = downloadHandlerTexture.texture;
        BG.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero);
    }

    /// <summary>
    /// 加载已有小剧场
    /// </summary>
    public void LoadDialogueButton()
    {
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.InitialDirectory = defaultPath;
        ofd.Title = "选择小剧场";
        ofd.Filter = "可用小剧场文件|*.yaml";

        if (ofd.ShowDialog() == DialogResult.OK)
        {
            BGImageFullPath = ofd.FileName;


            StartCoroutine(loadDialogue(BGImageFullPath));
        }
    }

    /// <summary>
    /// 加载已用小剧场用
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private IEnumerator loadDialogue(string path)
    {
        Deserializer deserializer = new Deserializer();

        //清单文件获取
        selectedDialogue = deserializer.Deserialize<YamlReadWrite.Dialogue>(File.ReadAllText(path));
        //读取内容
        if (selectedDialogue.extraContent != "可选内容")
        {
            Debug.Log("S");
            ExtraField.text = selectedDialogue.extraContent;
        }
        else
        {
            ExtraField.text = string.Empty;
        }
      
        leftField.text = selectedDialogue.LeftContent;
        rightField.text = selectedDialogue.RightContent;
        writerField.text = selectedDialogue.Writer;
        leftCharacter.GetSpriteFromAtlas($"Euphonium-characters_{selectedDialogue.characterLeft}");
        rightCharacter.GetSpriteFromAtlas($"Euphonium-characters_{selectedDialogue.characterRight}");
//储存数据，便于以后进行保存
        SetRightCharacterId(selectedDialogue.characterRight);
        SetLeftCharacterId(selectedDialogue.characterLeft);
        
        //更改path，使其仅有文件夹路径
        path = System.IO.Path.GetDirectoryName(path);
        //得到图片
        yield return LoadImage($"{path}/{selectedDialogue.BackgroundImageName}.jpg");
    }

    public void SetRightCharacterId(int id)
    {
        rightCharacterId = id;
    }

    public void SetLeftCharacterId(int id)
    {
        leftCharacterId = id;
    }
}