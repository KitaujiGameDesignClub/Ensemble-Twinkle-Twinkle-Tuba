using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 负责游戏中文字的动态变化和成果记录
/// </summary>
public class TextUI : MonoBehaviour
{
    public static TextUI textUI;

    /// <summary>
    /// 按钮点击的音效
    /// </summary>
    public AudioClip ClickEffect;
    
    /// <summary>
    /// 记录视频帧数与帧率
    /// </summary>
    public TMP_Text frameCount;

    // public TMP_Text Accuracy;

    /// <summary>
    /// 得分
    /// </summary>
    public TMP_Text score;

    
   
    /// <summary>
    /// 正确按下按键的个数
    /// </summary>
    private int right;
  
    /// <summary>
    /// 总共有多少按键
    /// </summary>
    private int total;

    private int fps;

    /// <summary>
    /// 多长时间更新一次fps
    /// </summary>
    private const float fpsUpdateTime = 0.5f;

    /// <summary>
    /// fps显示之前累积的RealTimeDelta
    /// </summary>
    private float totalRealTimeDeltaBeforeFpsShow;

    /// <summary>
    /// 累积了几个RealTimeDelta
    /// </summary>
    private int countTimeDelta;

    private void Awake()
    {
        textUI = this;
        total = 0;
        right = 0;
        score.text = "";
    }

    // Start is called before the first frame update
    private void Start()
    {
        frameCount.text = string.Empty;
    }

    // Update is called once per frame
    public void Update()
    {
        //时间够了，显示一次fps
        if (totalRealTimeDeltaBeforeFpsShow >= fpsUpdateTime)
        {
            fps = (int)(countTimeDelta / totalRealTimeDeltaBeforeFpsShow);
            countTimeDelta = 0;
            totalRealTimeDeltaBeforeFpsShow = 0f;
        }
        else
        {
            countTimeDelta++;
            totalRealTimeDeltaBeforeFpsShow += Time.unscaledDeltaTime;
        }

        frameCount.text = $"fps:{fps.ToString()}\nframe:{StaticVideoPlayer.videoPlayer.frame}";
    }

    /// <summary>
    /// 触碰期间，判断玩家是否按下相应的按键（活塞），计算准确率
    /// </summary>
    /// <param name="inTime"></param>
    public void CheckPressedInTime(bool inTime)
    {
        if (inTime)
        {
            right++;
           total++;
        }
        else total++;

        float scoreCache = (float)right / total;
   

        score.text = $"人群呼声：{((int)(scoreCache * 100)).ToString()} / 100";

    }
    

    
    public void ExitGame()
    {
        Settings.SaveSettings();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ReturnToTitle()
    {
        PublicAudioSource.PlaySoundEffect(ClickEffect);
        SceneManager.LoadScene("Opening");
        Settings.SaveSettings();
    }

    public void PlayAgain()
    {
        PublicAudioSource.PlaySoundEffect(ClickEffect);
        SceneManager.LoadScene("load");
        Settings.SaveSettings();
    }
}