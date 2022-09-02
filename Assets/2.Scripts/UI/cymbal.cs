using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cymbal : MonoBehaviour
{
    /// <summary>
    /// 两个大镲的初始位置（世界）
    /// </summary>
    private Vector2[] cymbalInitialPos = new Vector2[2];

    /// <summary>
    /// 两个大镲在一起的位置（世界）
    /// </summary>
    private Vector2[] togetherPos = new Vector2[2];

    public Transform[] cymbals;

    private WaitForEndOfFrame waitClick;

    private void Awake()
    {
        //得到初始位置
        for (int i = 0; i < 2; i++)
        {
            cymbalInitialPos[i] = cymbals[i].position;
        }

//设置碰在一起的坐标
        togetherPos[0] =
            transform.TransformPoint(new Vector3(-0.1f, cymbals[0].localPosition.y, cymbals[0].localPosition.z));
        togetherPos[1] =
            transform.TransformPoint(new Vector3(0.1f, cymbals[1].localPosition.y, cymbals[1].localPosition.z));
    }

    /// <summary>
    /// 按下左ctrl了
    /// </summary>
    public void LeftCtrl()
    {
        StopAllCoroutines();
        //每次都重置一下位置
        for (int i = 0; i < 2; i++)
        {
            cymbals[i].position = cymbalInitialPos[i];
        }

        StartCoroutine(click());
    }

    IEnumerator click()
    {
        //打
        while (true)
        {
            for (int i = 0; i < 2; i++)
            {
                cymbals[i].position = Vector2.Lerp(cymbals[i].position, togetherPos[i], 30f * Time.deltaTime);
            }

            if (cymbals[1].position.x - togetherPos[1].x <= 0.001f)
            {
                break;
            }

            yield return waitClick;
        }


        //收
        while (true)
        {
            for (int i = 0; i < 2; i++)
            {
                cymbals[i].position = Vector2.Lerp(cymbals[i].position, cymbalInitialPos[i], 25f * Time.deltaTime);
            }

            if (cymbals[1].position.x - togetherPos[1].x >= 0.3f)
            {
                break;
            }

            yield return waitClick;
        }
    }
}