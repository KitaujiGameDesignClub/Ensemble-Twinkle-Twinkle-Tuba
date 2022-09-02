using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    private static bool checkHasCLeared = false;
    
   private static List<IUpdate> Updates = new List<IUpdate>();

    private void Awake()
    {
        checkHasCLeared = false;
        Updates.Clear();
        checkHasCLeared = true;
    }

    public static void RegisterUpdate(IUpdate update)
    {
        if (checkHasCLeared)
        {
            Updates.Add(update);
        }
        else
        {
            Debug.LogError("意外的IUpdate注册。应晚于Awake执行");
        }
    }

    public static void Remove(IUpdate update)
    {
        Updates.Remove(update);
    }
    
    
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Updates.Count; i++)
        {
            Updates[i].FastUpdate();
        }
    }
}
