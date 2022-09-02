using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChooseGirls : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private CanvasGroup characterInf;

    private void Awake()
    {
        characterInf = GetComponent<CanvasGroup>();
        //开始游戏的时候，选择角色的界面都要消失
        characterInf.alpha = 0f;
    }

    /// <summary>
    /// 鼠标悬浮在那个角色上面，显示角色信息
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        characterInf.alpha = 1f;
    }

    /// <summary>
    /// 鼠标离开之后，隐藏角色信息
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void OnPointerExit(PointerEventData eventData)
    {
        characterInf.alpha = 0f;
    }

    /// <summary>
    /// 相应按钮，表明选择角色。 0 1 2与背景图相同
    /// </summary>
    /// <param name="characterId"></param>
    public void selected(int characterId)
    {
#if UNITY_EDITOR
        switch (characterId)
        {
            case 0:
                Debug.Log("选择了：川岛 绿辉");
                break;

            case 1:
                Debug.Log("选择了：加藤 叶月");
                break;
            case 2:
                Debug.Log("选择了：黄前 久美子");
                break;
        }
#endif
    }
}