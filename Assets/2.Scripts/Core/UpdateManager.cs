using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    private static bool checkHasCLeared = false;
    
   private static List<IUpdate> Updates = new List<IUpdate>();
   private static List<ILateUpdate> lateUpdates = new List<ILateUpdate>();


    private void Awake()
    {
        checkHasCLeared = false;
        Updates.Clear();
        lateUpdates.Clear();
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
#if UNITY_EDITOR
            Debug.LogError("意外的IUpdate注册。应晚于Awake执行");
#endif 
        }
    }
    
    public static void RegisterLateUpdate(ILateUpdate update)
    {
        if (checkHasCLeared)
        {
            lateUpdates.Add(update);
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogError("意外的IUpdate注册。应晚于Awake执行");
#endif 
        }
    }

    public static void Remove(IUpdate update)
    {
        Updates.Remove(update);
    }
    public static void RemoveLateUpdate(ILateUpdate update)
    {
       lateUpdates.Remove(update);
    }
    
    
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Updates.Count; i++)
        {
            Updates[i].FastUpdate();
        }
    }

    private void LateUpdate()
    {
        for (int i = 0; i < lateUpdates.Count; i++)
        {
            lateUpdates[i].BetterLateUpdate();
        }
    }
}
