using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 44拍子的节拍器
/// </summary>
public class Metronome : MonoBehaviour
{
    [Range(30,244)]
    public int bpm;
    
  
    /// <summary>
    /// 节拍  0表示尚未开始打拍子
    /// </summary>
    [HideInInspector]  public int meter = 0;
   
    /// <summary>
    /// 开始时间偏移（仅推迟）
    /// </summary>
    public float startTimeOffset = 0f;

    
    /// <summary>
    /// 节拍器准备开始工作时的事件（startTimeOffset秒之后开始第一次滴答）
    /// </summary>
    [Header("节拍器准备开始工作时的事件")]
    public UnityEvent OnReady = new();
/// <summary>
/// 每次滴答一下后调用的事件
/// </summary>
   [Header("每次滴答一下后调用的事件")]
    public UnityEvent<int> AfterTick = new();
    
    private bool isPlaying;
    


    public void StartPlay()
    {
        if(isPlaying) return;
        isPlaying = true;
        OnReady.Invoke();
        InvokeRepeating(nameof(Play),startTimeOffset,60f / bpm);
    }

    public void Stop()
    {
        isPlaying = false;
        CancelInvoke();
    }
 
   /// <summary>
   /// 按照拍子播放音效
   /// </summary>
   private void Play()
   {
       //因为是从0开始的，所以一上来就要加一个
      if(meter == 4) meter = 0;
      meter++;
      
        AfterTick.Invoke(meter);
    }

   
}
