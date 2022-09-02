using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 在确保物体不会被禁用或者摧毁前，继承本接口
/// </summary>
public interface IUpdate
{
   public void FastUpdate();
}
