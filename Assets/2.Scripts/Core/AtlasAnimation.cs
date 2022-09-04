using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AtlasAnimation : AtlasRead
{
    [Header("系列动画精灵名字")] public string AnimationName;
    [Header("动画精灵范围")] public int[] Range = new int[2];
    [Header("初始图片序号")] public int initialIndex;
    [Header("动画精灵名格式")] public string format;
    [Header("动画图片间隔")] public float interval;

    /// <summary>
    /// 不循环动画分组（数值为每个分组的最后一个的序号）
    /// </summary>
    [Header("不循环动画分组（数值为每个分组的最后一个的序号）")] public int[] groups;

    private GameObject go;

    /// <summary>
    /// 第几张图片
    /// </summary>
    private int index;

    /// <summary>
    /// 动画第几组
    /// </summary>
    private int groupIndex;

    private WaitForSeconds animatorInterval;

    public override void Awake()
    {
        base.Awake();
        go = gameObject;
        //初始图片设置为玩家设定的初始图片（按序号）
        index = initialIndex;
        //根据初始图片得到目前的分组序号
        for (int i = 0; i < groups.Length; i++)
        {
            if (groups[i] < initialIndex) continue;
            groupIndex = i;
            break;
        }

        //清除原有图片
        spriteRenderer.sprite = null;
    }

    public void Start()
    {
        //初始化动画间隔
        animatorInterval = new WaitForSeconds(interval);
    }

    /// <summary>
    /// 显示下一个图片
    /// </summary>
    [ContextMenu("显示下一个图片")]
    public void ShowNextAnimation()
    {
        if (index == Range[1] + 1) index = Range[0];

        spriteName = string.Format(format, AnimationName, index.ToString());
        GetSpriteFromAtlas();
        index++;
    }

    [ContextMenu("显示下一组动画（不循环）")]
    public void ShowNextGroupAnimation()
    {
        //先停止所有可能有的换图片的协程
        StopAllCoroutines();
        //避免分组超出预设
        if (groupIndex >= groups.Length) groupIndex = 0;
        //获取本组第一个图片的序号
        if (groupIndex != 0)
        {
            index = groups[groupIndex - 1] + 1;
        }
        else
        {
            index = Range[0];
        }

        groupIndex++;
        //防止物体被禁用后仍然要调用协程
        if (go.activeInHierarchy) StartCoroutine(ShowGroup(false));
    }

    /// <summary>
    /// 能循环播放的动画
    /// </summary>
    [ContextMenu("播放循环动画")]
    public void CycableAnimation()
    {
        //先停止所有可能有的换图片的协程
        StopAllCoroutines();
        if (go.activeInHierarchy) StartCoroutine(ShowGroup(true));
       
    }
    

    /// <summary>
    /// 显示一组动画
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowGroup(bool allowCycle)
    {
      
        while (true)
        { 
            ShowNextAnimation();
            
            if (allowCycle)
            {

                //避免分组超出预设
                if (groupIndex >= groups.Length) groupIndex = 0;
              
            }
            else
            {
               
                //groupIndex - 1:前文已经加了一个了，这边为了得到这一组所以减去
                if (index == groups[groupIndex - 1] + 1) break;
               
            }
            yield return animatorInterval;
        }
 
      
    }


}