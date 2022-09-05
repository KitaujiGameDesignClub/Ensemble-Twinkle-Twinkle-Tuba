using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrassInstruments : MonoBehaviour,IUpdate
{
    //按照三键系统进行。多的那个键用于进行吸气吹气判定
    
    public KeyCode[] keyCodes = new KeyCode[4];
    public Transform[] keys = new Transform[4];
   /// <summary>
   /// 按压速度
   /// </summary>
    public float pressSpeed = 25f;
    private bool[] keysPressed = new bool[4];

    private Vector3[] initialLocalPos;
    /// <summary>
    /// 铜管按下去之后的位置
    /// </summary>
    public Vector3[] pressedLocalPos;

    private void Awake()
    {
        if (keyCodes.Length != keys.Length)
        {
            Debug.LogErrorFormat($"错误：{gameObject.name}的KeyCodes与keys长度不一致");
            return;
        }
        //修正长度与初始化数组
        keysPressed = new bool[keyCodes.Length];
        initialLocalPos = new Vector3[keyCodes.Length];

        //获取按键初始位置
        for (int i = 0; i < keys.Length; i++)
        {
            initialLocalPos[i] = keys[i].localPosition;
        }
    }

  
    /// <summary>
    /// 按键和吹气吸气状态更新
    /// </summary>
    public void FastUpdate()
    {
        //更新输入状态，在下面进行动画更新
        for (int i = 0; i < keyCodes.Length; i++)
        {
            keysPressed[i] = Input.GetKey(keyCodes[i]);
        }
        //动画更新
        for (int i = 0; i < keys.Length; i++)
        {
            if (keysPressed[i])
            {
                keys[i].localPosition = Vector3.Lerp(keys[i].localPosition,pressedLocalPos[i],pressSpeed * Time.deltaTime);
            }
            else
            {
                keys[i].localPosition = Vector3.Lerp(keys[i].localPosition,initialLocalPos[i],pressSpeed * Time.deltaTime);
            }
        }
    }
    public void OnEnable()
    {
        UpdateManager.RegisterUpdate(this);
    }

    public void OnDisable()
    {
       UpdateManager.Remove(this);
    }
}
