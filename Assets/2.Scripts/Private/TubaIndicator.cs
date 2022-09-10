using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TubaIndicator : MonoBehaviour
{
   public readonly Color dark = new(0.33f, 0.33f, 0.33f);
   public readonly Color light = new Color(1f, 1f, 1f);

   private Image image;
   
   
   private void Awake()
   {
      image = GetComponent<Image>();
      image.color = dark;
   }

   private void Start()
   {
      image.color = dark;
     
   }

   /// <summary>
   /// 按键正确，点亮大号
   /// </summary>
   public void Twinkle()
   {
      //点亮大号
    
      image.color = light;
      //然后逐渐变暗
      
   }

   /// <summary>
   /// 大号变暗（每个视频帧执行）
   /// </summary>
   public void BecomingDark()
   {
   
     image.color = Color.Lerp( image.color, dark, 0.04f);
   }
}
