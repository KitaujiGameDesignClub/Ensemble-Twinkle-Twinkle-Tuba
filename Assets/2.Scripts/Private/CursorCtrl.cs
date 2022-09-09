using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 挂在Staff上实际是控制staff移动的
/// </summary>
public class CursorCtrl : MonoBehaviour
{


    
  


    /// <summary>
    /// 所有位点
    /// </summary>
    public Transform[] cursorLocation;

    /// <summary>
    /// 要按下去的指法
    /// </summary>
    public Core.Fingering[] fingeringNeedToPressed;

    /// <summary>
    /// 正确按下了对应的按键（指法）
    /// </summary>
    public UnityEvent onRight = new();
    
  /// <summary>
    /// 帧数间隔
    /// </summary>
    private int[] IntervalFrame;
  /// <summary>
  /// 第几个音符
  /// </summary>
    private int index;
    private float[] speeds;
    /// <summary>
    /// 按下按键了吗？
    /// </summary>
    private bool pressed;
    private Transform staff;




    /// <summary>
    /// 设定移动速度（绝对值）
    /// </summary>
    public void SetSpeed(Transform cursor)
    {
        staff = transform;

        speeds = new float[cursorLocation.Length];

        for (int i = 0; i < speeds.Length; i++)
        {
            //第一个音符的话，要按照光标的初始位置来计算速度
            if (i == 0)
            {
                speeds[0] = (cursorLocation[0].position.x - cursor.position.x) / IntervalFrame[0];
            }
            else
            {
                speeds[i] = (cursorLocation[i].position.x - cursorLocation[i - 1].position.x) / IntervalFrame[i];
            }
        }

    
    }


    /// <summary>
    /// 用于刷新（移动）乐谱（每个视频帧都调用，用事件）
    /// </summary>
    /// <param name="cursor">光标的变换组件</param>
    /// <returns>乐谱的位置</returns>
    public void StaffRefresh(Transform cursor)
    {
        //最后一个音符之后，乐谱不动了
        if (index < cursorLocation.Length)
        {
            staff.Translate(speeds[index] * Vector2.left);
            checkCursorPosition(cursor);
        }
        
        //进行要输入的按键的判定（这里仅针对那些要求玩家啥也不输入的）
       if (fingeringNeedToPressed[index] == Core.Fingering.Null)
        {
            checkFingeringsNeedToPressed(Core.Fingering.Null);
        }
       
       //低音提琴用 ：提示圈圈出来要按的键
      
    }


    /// <summary>
    /// 设定乐谱音符的时间间隔（从本地读取）
    /// </summary>
    public void SetInterval()
    {
        var yaml = YamlReadWrite.Read<YamlReadWrite.StaffTime>((YamlReadWrite.FileName)Core.selectedInstrument);

        //转化为程序可以识别的帧数
        IntervalFrame = new int[yaml.time.Length];
        for (int i = 0; i < IntervalFrame.Length; i++)
        {
            if (i == 0)
            {
                IntervalFrame[i] = YamlReadWrite.ConvertFriendlyToReadable(24, yaml.time[i],yaml.lag) - Episode.StartMoving;
            }
            else
            {
                IntervalFrame[i] = YamlReadWrite.ConvertFriendlyToReadable(24, yaml.time[i],yaml.lag) -
                                   YamlReadWrite.ConvertFriendlyToReadable(24, yaml.time[i - 1],yaml.lag);
   
            }
        }
    }
  
    /// <summary>
    /// 检查光标是不是到位点了。到了Index+1
    /// </summary>
    private void checkCursorPosition(Transform cursor)
    {
        if (cursorLocation[index].position.x - cursor.position.x <= 0.001f)
        {
            index++;
        }
    }
    
    /// <summary>
    /// 检查指法是否对应（三个乐器都能用，不想改名字了）
    /// </summary>
    /// <param name="fingerings"></param>
    /// <returns></returns>
    public void CheckFingeringForBass(int fingering)
    {
        //这里，只要玩家一旦按下去（在到达不需要按键的音符之前），就会让pressed为true
        pressed = fingeringNeedToPressed[index] == Core.Fingering.Null;
       
        
        checkFingeringsNeedToPressed((Core.Fingering)fingering);
    }

    /// <summary>
    /// 检查指法是否对应（bass用）
    /// </summary>
    /// <param name="fingering">接受玩家的fingering输入</param>
    /// <param name="index">第几个音符</param>
    /// <returns></returns>
    bool CheckFingering(Core.Fingering fingering,int index)
    {
       
        
        return fingering == fingeringNeedToPressed[index];
  
    }
    
    /// <summary>
    /// 专门用来接受玩家输入和检查指法。还有bass在对应的按钮那边显示提示圈
    /// </summary>
    public void FastUpdate()
    {
       
    }
    
    /// <summary>


    /// <summary>
    /// 检查音符目前要按的键
    /// </summary>
    private void checkFingeringsNeedToPressed(Core.Fingering fingering)
    {
        //既然是要求玩家啥也不输入的，就只要让玩家保持 在到达这个不需要输入的音符之前，啥都不输入就行
        if (fingering == Core.Fingering.Null)
        {
            if (!pressed)
            {
                onRight.Invoke();
            }
        }
        
        
        //确定应该是第几个音符
        switch (index)
        {
            //0这一情况，单独拿出来
            case 0:
                if (StaticVideoPlayer.staticVideoPlayer.Frame - IntervalFrame[0] <= 5)
                {
                    //这第0个音符，刚好落在提前5帧的范围内
                    //检查玩家输入的指法是否符合要要求
                    if ( CheckFingering(fingering, 0))
                    {
                        onRight.Invoke();
                    }

                   
                }
                break;
                
            default:
                if (Mathf.Abs(StaticVideoPlayer.staticVideoPlayer.Frame - IntervalFrame[index]) <= 5)
                {
                    //第index个音符
                    //检查玩家输入的指法是否符合要要求
                    if ( CheckFingering(fingering, index))
                    {
                        onRight.Invoke();
                    }
                }
                else if (Mathf.Abs(StaticVideoPlayer.staticVideoPlayer.Frame - IntervalFrame[index - 1]) <= 5)
                {
                    //第index - 1个音符
                    //检查玩家输入的指法是否符合要要求
                    if ( CheckFingering(fingering, index - 1))
                    {
                        onRight.Invoke();
                    }
                }

                break;
        }

    }
    
    


#if UNITY_EDITOR
    [ContextMenu("获取所有位点的x")]
    public void GetAllCursorPosition()
    {
        var trs = GetComponentsInChildren<Transform>();

        cursorLocation = new Transform[trs.Length - 1];


        for (int i = 0; i < trs.Length - 1; i++)
        {
            cursorLocation[i] = trs[i + 1];
        }
    }
#endif
  
}