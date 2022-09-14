using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ScreenButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
   
  [HideInInspector]  public bool OnPressed;
 

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnPressed = false;
    }
}
