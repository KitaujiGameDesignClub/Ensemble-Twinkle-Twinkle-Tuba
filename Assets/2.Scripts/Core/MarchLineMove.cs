
using UnityEngine;
using UnityEngine.Events;

public class MarchLineMove : MonoBehaviour,IUpdate
{
    private Transform tr;
   private Vector2 initialPos;
   public float moveSpeed;
   /// <summary>
   /// 步伐那边，判定区域用的
   /// </summary>
   [Header("步伐部分")] 
   /// <summary>
   /// 步伐判定射线位置
   /// </summary>
   public Transform[] marchLineRayPos;


   public UnityEvent inTime = new UnityEvent();
   public UnityEvent miss = new();

   /// <summary>
   /// 按键错过了吗
   /// </summary>
   private bool buttonMiss = true;

   private int index;


   /// <summary>
   /// 第一次return之后变为true，然后正常游戏。在此之前这个移动的线不显示，也不判定得分
   /// </summary>
   private bool ready;
   
   private void Awake()
    {
        tr = transform;
        initialPos = tr.position;
        
    }

    //平日是被金庸的。一旦被激活就开始运动
   public void Start()
    {
        UpdateManager.RegisterUpdate(this);
    }

   public void PrepareToReturn()
   {
       if (index == 0)
       {
           index++;
       }
       else
       {
           index = 0;
           Return();
       }
   }
   
   
   
/// <summary>
/// 返回。两种调用：外部（节拍器）调用意味着玩家没有及时按下；反之就是及时按下
/// </summary>
/// <param name="miss"></param>
     void Return()
    {
        if (!ready)
        {
            ready = true;
            tr.position = initialPos;
            return;
        }
        
        
        if(buttonMiss) this.miss.Invoke();
      tr.position = initialPos;
      buttonMiss = true;
      
      /*
       * 这段代码的意思是：
       * 如果玩家及时按下了按键（ FastUpdate()中的if判定成功），回车，但是if中执行按键及时的事件；节拍器调用时仅返回到初始位置
       * 如果玩家没有及时按下，这个方法会被节拍器调用，buttonMiss默认为true，就执行miss事件，并返回到初始位置
       */

    }
    
/// <summary>  
/// 不断向左移动，移动出左侧的提示线一定距离就行
/// </summary>
public void FastUpdate()
{
    bool ray = Physics.Raycast(marchLineRayPos[0].position, Vector3.forward, 0.5f, 1 << 6) ||
               Physics.Raycast(marchLineRayPos[1].position, Vector3.forward, 0.5f, 1 << 6);

    //射线与自己（移动的判定线）触碰，并且玩家按下了对应的按键
    if (ray && Input.GetKeyDown(KeyCode.RightControl))
    {
        inTime.Invoke();
        buttonMiss = false;
        return;
    }


   if(ready)  tr.Translate(moveSpeed * Time.deltaTime * Vector2.left);
      
    }

    public void MarchEnd()
    {
        UpdateManager.Remove(this);
        Destroy(gameObject);
    }
}
