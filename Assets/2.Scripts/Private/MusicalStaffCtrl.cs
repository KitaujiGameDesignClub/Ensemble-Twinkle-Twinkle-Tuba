using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalStaffCtrl : MonoBehaviour
{
    /// <summary>
    /// staff开始移动的滞后时间
    /// </summary>
    public float startTimeOffset;
    
    [Header("此物体一旦被激活，乐谱就开始移动了")]
    [Space(20)]
    [Header("目前到第几个节拍了。1-4")]
    [Header("0：低音提琴")]
    [Header("1：大号")]
    [Header("2：上低音号")]
    public int[] index;

    [Header("节拍间隔距离偏移")] 
    public float[] DistanceOffsetForEupho = new float[4];
    public float[] DistanceOffsetForTuba = new float[4];
    public float[] DistanceOffsetForBass = new float[4];

    [Header("允许乐谱每动一次，游戏就暂停吗？")]
    public bool pauseWhenStaffMoved;

 //   [Header("节拍器。在这里调整BPM")]
    //public Metronome metronome;
    
[Header("允许节拍器播放音效吗")]
    public bool allowSoundEffect = false;
[Header("节拍器音效")]
    public AudioClip soundeffect;

        [Header("三个乐器")]
    public GameObject[] instruments = new GameObject[3];
    [Header("三个乐谱")]
    public CursorCtrl[] staffs = new CursorCtrl[3];

    [Header("辅助线 光标")] 
    public Transform Cursor;
    
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //先禁用乐器和乐谱
        for (int i = 0; i < 3; i++)
        {
            instruments[i].SetActive(false);
            staffs[i].gameObject.SetActive(false);
        }
        //禁用判定线
        Cursor.gameObject.SetActive(false);
        //播放视频
        StaticVideoPlayer.staticVideoPlayer.Play();
        Invoke(nameof( showStaffAndInstrument),20f);

        //   metronome.OnReady.AddListener(showStaffAndInstrument);
        //  metronome.AfterTick.AddListener(moveStaff); 
        //  metronome.StartPlay();
    }

    // Update is called once per frame


    /// <summary>
    /// 展示乐谱和乐器
    /// </summary>
    void showStaffAndInstrument()
    {
       
       //显示乐器
        instruments[Core.selectedInstrument].SetActive(true);
        //显示乐谱
        staffs[Core.selectedInstrument].gameObject.SetActive(true);
        //计算乐谱移动速度
        staffs[Core.selectedInstrument].SetSpeed(Cursor);
        //显示判定线
       Cursor.gameObject.SetActive(true); 
      
    
    //一段时间之后注册事件，让乐谱移动
   Invoke(nameof(register),startTimeOffset);
    }

    void register()
    {
        StaticVideoPlayer.staticVideoPlayer.eachFrame.AddListener(delegate { staffs[Core.selectedInstrument].StaffRefresh(Cursor); });
    }
    
    
  
    
    
    /// <summary>
    /// 移动乐谱
    /// </summary>
    [Obsolete("旧的，按节奏动的")]
    void moveStaff(int meter)
    {
        //记录节拍数
        index[Core.selectedInstrument] = meter;
        //播放音效
      if(allowSoundEffect)   audioSource.PlayOneShot(soundeffect);
        //移动乐谱

        switch (Core.selectedInstrument)
        {
            case 0:
          //      staffs[Core.selectedInstrument].Translate(Vector3.left * (0.4f + DistanceOffsetForBass[index[Core.selectedInstrument] - 1]) );//原来的速度是0.32
                break;
            
            case 1:
        //        staffs[Core.selectedInstrument].Translate(Vector3.left * (0.4f + DistanceOffsetForTuba[index[Core.selectedInstrument] - 1]) );
                break;
            case 2:
         //       staffs[Core.selectedInstrument].Translate(Vector3.left * (0.4f + DistanceOffsetForEupho[index[Core.selectedInstrument] - 1]) );
                break;
        }

      
       
       


    }
}
