using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PublicAudioSource : MonoBehaviour
{
    public static PublicAudioSource publicAudioSource;

    /// <summary>
    /// 点击音效（长度大于1则随机播放一个）
    /// </summary>
    public AudioClip[] clicks;

    public enum AudioType
    {
        Click,
    }
    
    private  AudioSource Music;
    private  AudioSource Effect;
    
    private void Awake()
    {
        publicAudioSource = this;
        DontDestroyOnLoad(gameObject);
        
        AudioSource[] audio = GetComponents<AudioSource>();
        if (audio[0].playOnAwake)
        {
            Music = audio[0];
            Effect = audio[1];
        }
        else
        {
            Music = audio[1];
            Effect = audio[0];
        }
        
        //停止播放
        Music.Stop();
    }
    

    // Start is called before the first frame update
    public void PlayBackgroundMusic(AudioClip clip)
    {
        //停止之前的播放
        StopMusicPlaying();
        Music.clip = clip;
        UpdateMusicVolume();
        Music.Play();
    }

    public  void UpdateMusicVolume()
    {
        Music.volume = Settings.SettingsContent.MusicVolume;
    }

    public void StopMusicPlaying()
    {
        Music.Stop();
        Music.clip = null;
    }

    /// <summary>
    /// 播放音效（公用）
    /// </summary>
    /// <param name="clip"></param>
    public  void PlaySoundEffect(AudioType type)
    {
        switch (type)
        {
            case AudioType.Click:
               PlaySoundEffect(clicks[Random.Range(0,clicks.Length)]);
                break;
                
        }
      
    }

    /// <summary>
    /// 播放音效（指定）
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySoundEffect(AudioClip clip)
    {
        Effect.Stop();
        Effect.PlayOneShot(clip,Settings.SettingsContent.SoundEffectVolume);
    }

    public void SoundEffectStop()
    {
        Effect.Stop();
    }
    
}
