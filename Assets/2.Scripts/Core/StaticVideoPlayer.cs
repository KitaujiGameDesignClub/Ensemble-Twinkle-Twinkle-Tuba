using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// 
/// </summary>
public class StaticVideoPlayer : MonoBehaviour
{
    public static VideoPlayer videoPlayer;
    
    private AudioSource audioSource;


    private void Awake()
    {
       
        videoPlayer = GetComponent<VideoPlayer>();
        audioSource = GetComponent<AudioSource>();

        
        audioSource.volume = Settings.SettingsContent.MusicVolume;
    }


    
  
    //音量控制，用事件组
}
