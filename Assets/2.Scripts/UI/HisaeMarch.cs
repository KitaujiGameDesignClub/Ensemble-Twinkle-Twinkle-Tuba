using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HisaeMarch : MonoBehaviour,IUpdate
{
    private Transform tr;
    /// <summary>
    /// 结束的位置
    /// </summary>
    public Vector2 EndPos;
    /// <summary>
    /// 最初的位置
    /// </summary>
    private Vector2 initialPos;

    private void Awake()
    {
        tr = transform;
        initialPos = tr.position;
    }

    /// <summary>
    /// 一旦被激活，就开始往前走
    /// </summary>
    private void Start()
    {
        UpdateManager.RegisterUpdate(this);
    }

    public void FastUpdate()
    {
        //计算移动的百分比
        float percent = (float)(StaticVideoPlayer.videoPlayer.frame - Episode.StartMarch) /
                         (Episode.MarchEnd - Episode.StartMarch);
        if (percent >= 1f)
        {
            MarchEnd();
            return;
        }
        tr.position = new Vector2((EndPos.x - initialPos.x) * percent + initialPos.x,initialPos.y) ;

       
    }

    public void MarchEnd()
    {
        UpdateManager.Remove(this);
        Destroy(gameObject);
    }


}
