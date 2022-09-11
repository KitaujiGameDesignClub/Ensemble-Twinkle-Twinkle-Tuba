using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChooseGirls : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// 角色被选定后的方框
    /// </summary>
    public Image selectedBox;

    /// <summary>
    /// 角色选择音效
    /// </summary>
    public AudioClip selectedSound;


    private CanvasGroup characterInf;

    /// <summary>
    /// 被点击了
    /// </summary>
    private bool pressed;


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
        if (!pressed) characterInf.alpha = 0f;
    }


    /// <summary>
    /// 按了别人
    /// </summary>
    public void PressOthers()
    {
        pressed = false;
        //消除框框
        selectedBox.color = new Color(1f, 1f, 1f, 0f);
        //鼠标离开操作
        characterInf.alpha = 0f;
    }

    /// <summary>
    /// 相应按钮，表明选择角色。 0 1 2与背景图相同
    /// </summary>
    /// <param name="characterId"></param>
    public void selected(int characterId)
    {
        //防止多次执行
        if (pressed)
        {
            return;
        }

        selectedBox.color = Color.white;
//确定选择的角色
        Core.core.selectedInstrument = characterId;
        //音效播放
        PublicAudioSource.publicAudioSource.PlaySoundEffect(selectedSound);
        //自己被暗下去了
        pressed = true;
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