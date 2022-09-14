
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BrassInstruments : MonoBehaviour, IUpdate,ILateUpdate
{
    public int instrumentId;

    //按照三键系统进行。多的那个键用于进行吸气吹气判定

    /// <summary>
    /// 按键输入设定
    /// </summary>
    public KeyCode[] keyCodes = new KeyCode[4];
/// <summary>
/// 按键输入设定（Android用）
/// </summary>
    public ScreenButton[] screenButton = new ScreenButton[4];
    
    /// <summary>
    /// 活塞
    /// </summary>
    public Transform[] keys = new Transform[4];

    /// <summary>
    /// 按压速度
    /// </summary>
    public float pressSpeed = 25f;

    /// <summary>
    /// 呼气吸气指示器
    /// </summary>
    public Image BreathIndicator;

    /// <summary>
    /// 保存按键按下去的状态（一直按着）
    /// </summary>
    private bool[] keysPressed = new bool[4];
    

    private Vector3[] initialLocalPos;

    /// <summary>
    /// 铜管按下去之后的位置
    /// </summary>
    public Vector3[] pressedLocalPos;


    public CursorCtrl cursorCtrl;

    /// <summary>
    /// 气息指示器（透明颜色）
    /// </summary>
    private readonly Color transparent = new Color(1f, 1f, 1f, 0f);

    private PointerEventData pointerEventData;
    
    private void Awake()
    {
        //修正长度与初始化数组
        keysPressed = new bool[keyCodes.Length];
        initialLocalPos = new Vector3[keyCodes.Length];

        //获取按键初始位置
        for (int i = 0; i < keys.Length; i++)
        {
            initialLocalPos[i] = keys[i].localPosition;
        }

        //隐藏气息标记
        BreathIndicator.color = transparent;
        
      
#if !UNITY_ANDROID
        //删除不必要数组
        screenButton = null;
        
    
#endif
       
    }


    /// <summary>
    /// 按键和吹气吸气状态更新
    /// </summary>
    public void FastUpdate()
    {
        
        //更新输入状态，在下面进行动画更新
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        for (int i = 0; i < keyCodes.Length; i++)
        {
         
            keysPressed[i] = Input.GetKey(keyCodes[i]);
        }
        
    #elif UNITY_ANDROID
     for (int i = 0; i < screenButton.Length; i++)
        {
            keysPressed[i] = screenButton[i].OnPressed;
        }
        
#endif

       

      
    }

    public void OnEnable()
    {
        UpdateManager.RegisterUpdate(this);
        UpdateManager.RegisterLateUpdate(this);
        
    }

    public void OnDisable()
    {
        UpdateManager.Remove(this);
        UpdateManager.Remove(this);
    }

    public void BetterLateUpdate()
    {
          //针对铺面要求的指法的判断（大号）
        if (instrumentId == 1)
        {
            //每一个音符都有空格作为吹气keysPressed[3]=true
            switch (keysPressed[0])
            {
                //仅吹气（无按键）
                case false when !keysPressed[1] && !keysPressed[2] && keysPressed[3]:
                    cursorCtrl.CheckFingeringForBass(Core.Fingering.Space);
                 
                    break;
                //活塞13
                case true when keysPressed[2] && keysPressed[3]:
                    cursorCtrl.CheckFingeringForBass(Core.Fingering.Key13);
                  
                    break;
                //活塞1
                case true when keysPressed[3]:
                    cursorCtrl.CheckFingeringForBass(Core.Fingering.Key1);
                 
                    break;
            }
            //仅吹气
        }
        //针对铺面要求的指法的判断（上低音号）
        else
        {
            switch (keysPressed[0])
            {
                case false when !keysPressed[1] && !keysPressed[2] && !keysPressed[3]:
                    cursorCtrl.CheckFingeringForBass(Core.Fingering.Null);
                    
                    break;
                case true when keysPressed[1] && keysPressed[3]:
                    cursorCtrl.CheckFingeringForBass(Core.Fingering.Key12Space);
                  break;
                case true when keysPressed[1]:
                    cursorCtrl.CheckFingeringForBass(Core.Fingering.Key12);
                  break;
                case true when keysPressed[2]:
                    cursorCtrl.CheckFingeringForBass(Core.Fingering.Key13);
                  break;
                default:
                {
                    if (keysPressed[3])
                    {
                        cursorCtrl.CheckFingeringForBass(Core.Fingering.Space);
                    
                    }


                    else if (keysPressed[0])
                    {
                        cursorCtrl.CheckFingeringForBass(Core.Fingering.Key1);
                      
                    }

                    break;
                }
            }
        }


        //动画更新
        for (int i = 0; i < keyCodes.Length; i++)
        {
            //气息控制
            if (i == keyCodes.Length - 1)
            {
                if (keysPressed[i])
                {
                    //淡入气息标记

                    BreathIndicator.color = Color.white;
                }
                else
                {
                    //淡出气息标记
                    BreathIndicator.color = Color.Lerp(BreathIndicator.color, transparent, 20f * Time.deltaTime);
                    // if(BreathIndicator.color.a >= 0.99f)   BreathIndicator.CrossFadeAlpha(0F,0.1F,false);
                }

                return;
            }


            if (keysPressed[i])
            {
                keys[i].localPosition =
                    Vector3.Lerp(keys[i].localPosition, pressedLocalPos[i], pressSpeed * Time.deltaTime);
            }
            else
            {
                keys[i].localPosition =
                    Vector3.Lerp(keys[i].localPosition, initialLocalPos[i], pressSpeed * Time.deltaTime);
            }
        }
    }
}