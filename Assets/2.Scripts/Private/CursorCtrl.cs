using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 挂在Staff上实际是控制staff移动的
/// </summary>
public class CursorCtrl : MonoBehaviour
{
    /// <summary>
    /// 帧数间隔
    /// </summary>
    public int[] IntervalFrame;


    /// <summary>
    /// 所有位点
    /// </summary>
    public Transform[] cursorLocation;

    private int index;
    private float[] speeds;

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

        //清除不需要的数组，节省点内存
        IntervalFrame = null;
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
    }


    /// <summary>
    /// 检查光标是不是到位点了。到了Index+1
    /// </summary>
    private void checkCursorPosition(Transform cursor)
    {
        if (cursorLocation[index].position.x - cursor.position.x <= 0.001f)
        {
            Debug.Log(cursorLocation[index].position.x +"   "+ staff.position.x);
            
            Debug.Log("sdsada");
            index++;
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