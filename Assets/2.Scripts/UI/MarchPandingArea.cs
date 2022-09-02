using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarchPandingArea : MonoBehaviour
{
    private Image spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<Image>();
    }

    // Start is called before the first frame update


    // Update is called once per frame
   public  void MarchPandingAreaFadeOut()
    {
        InvokeRepeating(nameof(fadeOut),2f,2f);
    }

   void fadeOut()
   {
       spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b,
           spriteRenderer.color.a - 0.2f);
   }
}
