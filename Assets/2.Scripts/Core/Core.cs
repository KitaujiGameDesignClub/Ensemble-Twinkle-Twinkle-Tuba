using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Core : MonoBehaviour,IUpdate
{
    [Header("大镲部分")]
    public Image clef;


    
    public  UnityEvent onGetButtonLeftCtrl = new UnityEvent();


    [Header("按键及时（大镲）")]
    public UnityEvent inTime = new UnityEvent();
   [Header("按键疏忽（大镲）")]
    public UnityEvent miss = new UnityEvent();

   
    [Header("视频播放")]
    public  UnityEvent onStart = new UnityEvent();
   [Header("领队吹哨")]
    public  UnityEvent onPrepare = new UnityEvent();
[Header("开始行进")]
    public UnityEvent onMarch = new UnityEvent();

    [Header("行进结束")] public UnityEvent onEnd = new();
    [Header("结算")] public UnityEvent onSettlement = new();
    
    /// <summary>
    /// 部分
    /// </summary>
    private int episode;
    
    /// <summary>
    /// 电脑可以识别的大镲时间（帧数）
    /// </summary>
    private List<int> cymbalAction;

    /// <summary>
    /// 到底几个大镲了（从0开始）0：高坂之前
    /// </summary>
    private int index;

   
    
    private void Awake()
    {
        Initialization();
        onStart.Invoke();

#if UNITY_EDITOR
        //编辑器模式下，永远允许跳过老师的话
        Settings.SettingsContent.hasPlayed = true;
#endif
        
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateManager.RegisterUpdate(this);
        StaticVideoPlayer.videoPlayer.Play();
    }

    // Update is called once per frame
   public void FastUpdate()
    {
        //游戏暂停预留
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale >= 0.99f)
            {
                Time.timeScale = 0f;
                StaticVideoPlayer.videoPlayer.Pause();
            }
            else
            {
                Time.timeScale = 1f;
                StaticVideoPlayer.videoPlayer.Play();
            }
        }
        //重新开始
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene("SampleScene");
        
        
        if(!StaticVideoPlayer.videoPlayer.isPlaying) return;
        

        if(Input.GetKeyDown(KeyCode.LeftControl)) onGetButtonLeftCtrl.Invoke();

       
        
        switch (StaticVideoPlayer.videoPlayer.frame)
        {
            //开始部分（用于制作跳过），到吹哨
            case < Episode.whistle when episode == 0:
                //空格允许跳过老师的话（在玩过之后）
                if (Input.GetKeyDown(KeyCode.Space) && Settings.SettingsContent.hasPlayed)
                {
                    StaticVideoPlayer.videoPlayer.frame = Episode.whistle;
                    episode++;
                }

                break;
            
       
            
            //吹哨开始
            case >= Episode.whistle and < Episode.MarchEnd when episode <= 3:

                if (episode == 0) episode = 1;
               
                
                //每帧都要执行的大镲判定
                ClefFadeInAndKeyCheck();
                //吹哨开始到结束，一些仅调用一次的事件
                switch (episode)
                {
                    case 1:
                        episode++;
                        //吹哨到踢腿行进这段，叫做准备
                        onPrepare.Invoke();
                        break;
                    //其中，有个开始踢腿
                    case 2 when StaticVideoPlayer.videoPlayer.frame >= Episode.StartMarch:
                        episode++; //3
                        onMarch.Invoke();
                        break;
                }
 
                break;
            //行进结束
            case >= Episode.MarchEnd when episode == 3:
                episode++;
                onEnd.Invoke();
               
#if !UNITY_EDITOR
//设置为已经玩过游戏了
           Settings.SettingsContent.hasPlayed = true;      
#endif
             
                break;
            //结算
            case >= Episode.Settlement when episode == 4:
                episode++;
                onSettlement.Invoke();
                break;
                
        }


     
        
        
    }


   private void Initialization()
   {
       //读取大镲文件
       ReadYaml();
       //clef消失（不填充）
       clef.fillAmount = 0f;
   }

/// <summary>
/// 读取大镲文件
/// </summary>
   private void ReadYaml()
   {
       var yaml = YamlReadWrite.Read<YamlReadWrite.CymbalAction>(YamlReadWrite.FileName.Cymbal);

       cymbalAction = new List<int>();
      
       for (int i = 0; i < yaml.time.Length; i++)
       {
           //按照冒号分开。长度为4. 0 =1舍弃 =2有改动（相较于AS的版本） .1是分钟（1min=60s=1800)  2是秒（1s=30） 3则可以视为帧数
           string[] fix = yaml.time[i].Split(':');
          
           //lag对于整体的滞后性进行修复
           cymbalAction.Add(int.Parse(fix[3]) + int.Parse(fix[2]) * 30 + int.Parse(fix[1]) * 1800 +  Settings.SettingsContent.lag);
           
           
       }


   }

   /// <summary>
   /// 音符（打击乐符号）淡入和按键检查
   /// </summary>
   private void ClefFadeInAndKeyCheck()
   {
       
       var frame = StaticVideoPlayer.videoPlayer.frame;
       
       //按照视频进度，对打击乐符号进行填充
       if (index == 0)
       {
           //775:老师说出展现北宇治的实力之前的部分episode = 0
           clef.fillAmount = (float)(frame - 973)  / (cymbalAction[index] - 973);
       }
       else
       {
           clef.fillAmount = (float)(frame - cymbalAction[index - 1])  / (cymbalAction[index] - cymbalAction[index - 1]);
       }
    
       
       

       //玩家按键判定
       if (frame >= cymbalAction[index] - 3 && frame <= cymbalAction[index] + 3 && Input.GetKeyDown(KeyCode.LeftControl))
       {
           if( index < cymbalAction.Count - 1)  index++;
           clef.fillAmount = 0f;
           inTime.Invoke();
       }
       //错过
       else if (frame > cymbalAction[index] + 3)
       {
           clef.fillAmount = 0f;
         if( index < cymbalAction.Count - 1)  index++;
           miss.Invoke();
       }

    
   }
   
   
#if UNITY_EDITOR
    [ContextMenu("间隔检查")]
    private void CheckInterval()
    {
        if (!EditorApplication.isPlaying)
        {
            Debug.LogWarning("间隔检查要在运行情况下进行");
            return;
            
        }
        
      
        for (int j = 1; j < cymbalAction.Count; j++)
        {
            if (cymbalAction[j] - cymbalAction[j - 1] <= 8)
            {
                Debug.LogError($"存在过短:{YamlReadWrite.Read<YamlReadWrite.CymbalAction>(YamlReadWrite.FileName.Cymbal).time[j]}与{YamlReadWrite.Read<YamlReadWrite.CymbalAction>(YamlReadWrite.FileName.Cymbal).time[j - 1]}");
            }
        }
    } 
#endif

   
}
