using System;
using Codice.CM.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

/// <summary>
/// 
/// </summary>
public class StaticVideoPlayer : MonoBehaviour,IUpdate
{
    public static StaticVideoPlayer staticVideoPlayer;
    
    
    public long Frame => VideoPlayer.frame;
    public  bool IsPlaying => VideoPlayer.isPlaying;

    public UnityEvent eachFrame;

    /// <summary>
    /// 记录视频上一帧是第几帧
    /// </summary>
    private long PreviousFrameInVideo;
    
    VideoPlayer VideoPlayer;
    
    private AudioSource audioSource;


    private void Awake()
    {

        staticVideoPlayer = this;
        VideoPlayer = GetComponent<VideoPlayer>();

        if (VideoPlayer.playOnAwake)
        {
            Debug.LogError("不允许PlayOnAwake");
            return;
        }
        
        audioSource = GetComponent<AudioSource>();

        
        audioSource.volume = Settings.SettingsContent.MusicVolume;

      
    }


    public  void Play()
    {
        VideoPlayer.Play();
        PreviousFrameInVideo = VideoPlayer.frame;
        UpdateManager.RegisterUpdate(this);
        
    }

    public void Pause()
    {
      UpdateManager.Remove(this);
        VideoPlayer.Pause();
    }

    /// <summary>
    /// 跳跃之后，每帧执行的事件失效
    /// </summary>
    /// <param name="frame"></param>
    public void Jump(long frame)
    {
        VideoPlayer.frame = frame;
      
    }

    /// <summary>
    /// 注册事件：每帧执行的要运行
    /// </summary>
    public void RegisterEachFrame()
    {
        PreviousFrameInVideo = VideoPlayer.frame - 1;
    }


    //音量控制，用事件组
    public void FastUpdate()
    {
       
        
        //每多一帧，调用一次方法
        if (VideoPlayer.frame - PreviousFrameInVideo == 1)
        {
           
            PreviousFrameInVideo++;
            eachFrame.Invoke();
        }
    }
}
