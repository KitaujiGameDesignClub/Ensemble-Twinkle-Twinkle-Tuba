using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInstrument
{
 
   
   public void Play(bool[] index);

   public void OnEnable();


   public void OnDisable();

}

