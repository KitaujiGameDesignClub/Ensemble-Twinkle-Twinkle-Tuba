
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpeningCtrl : MonoBehaviour
{
    public Slider MusicVolSlider;
    public Slider EffectVolSlider;

    public AudioClip bgm;
   
    public UnityEvent initialization = new();

    private void Awake()
    {
        Application.targetFrameRate = -1;
        //读取设置文件
        Settings.ReadSettings();
        initialization.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        //按照文件调整滑块
        MusicVolSlider.value = Settings.SettingsContent.MusicVolume;
        EffectVolSlider.value = Settings.SettingsContent.SoundEffectVolume;

        //调整音量
        PublicAudioSource.UpdateMusicVolume();
        
        //注册事件
        MusicVolSlider.onValueChanged.AddListener(delegate(float arg0)
        {
            Settings.SettingsContent.MusicVolume = arg0; PublicAudioSource.UpdateMusicVolume();
        });
        EffectVolSlider.onValueChanged.AddListener(delegate(float arg0)
        {
            Settings.SettingsContent.SoundEffectVolume = arg0;
        });

        
        //播放BGM
        PublicAudioSource.PlayBackgroundMusic(bgm);
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
    
    
    public void StartGame()
    {
        
     SceneManager.LoadScene("load");
    }

 
}
