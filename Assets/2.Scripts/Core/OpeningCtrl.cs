
using TMPro;
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

    public TMP_Text staff;
    
    private void Awake()
    {
        Application.targetFrameRate = -1;
        
        initialization.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        //按照文件调整滑块
        MusicVolSlider.value = Settings.SettingsContent.MusicVolume;
        EffectVolSlider.value = Settings.SettingsContent.SoundEffectVolume;
        
        
        //注册事件，滑条更新音量
        MusicVolSlider.onValueChanged.AddListener(delegate(float arg0)
        {
            Settings.SettingsContent.MusicVolume = arg0; PublicAudioSource.publicAudioSource.UpdateMusicVolume();
        });
        EffectVolSlider.onValueChanged.AddListener(delegate(float arg0)
        {
            Settings.SettingsContent.SoundEffectVolume = arg0;
        });

        
        //播放BGM
        PublicAudioSource.publicAudioSource.PlayBackgroundMusic(bgm);
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
        PublicAudioSource.publicAudioSource.PlaySoundEffect(PublicAudioSource.AudioType.Click);
     SceneManager.LoadScene("load");
    }

    public void clickSound()
    {
        PublicAudioSource.publicAudioSource.PlaySoundEffect(PublicAudioSource.AudioType.Click);
    }

    public void OpenWeb()
    {
        clickSound();
        Application.OpenURL("https://kitaujigamedesign.top");
    }

 
}
