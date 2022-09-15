using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScreenButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
   
  [HideInInspector]  public bool OnPressed;
  [HideInInspector] public bool onClick;

  private float pressedTime;
  
  
  
    public void OnPointerDown(PointerEventData eventData)
    {
        OnPressed = true;
       
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnPressed = false;
    }


  
}
