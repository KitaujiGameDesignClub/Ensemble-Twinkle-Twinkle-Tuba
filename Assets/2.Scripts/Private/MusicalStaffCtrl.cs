using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalStaffCtrl : MonoBehaviour
{
  
    
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

    [Header("节拍器。在这里调整BPM")]
    public Metronome metronome;
[Header("允许节拍器播放音效吗")]
    public bool allowSoundEffect = false;
[Header("节拍器音效")]
    public AudioClip soundeffect;

        [Header("三个乐器")]
    public GameObject[] instruments = new GameObject[3];
    [Header("三个乐谱")]
    public Transform[] staffs = new Transform[3];

    [Header("辅助线")] 
    public GameObject helpLine;
    
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
        helpLine.SetActive(false);
        
       
        metronome.OnReady.AddListener(showStaffAndInstrument);
        metronome.AfterTick.AddListener(moveStaff);
        metronome.StartPlay();
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
        //显示判定线
        helpLine.SetActive(true); 
    }

    /// <summary>
    /// 移动乐谱
    /// </summary>
    void moveStaff(int meter)
    {
        //记录节拍数
        index[Core.selectedInstrument] = meter;
        //播放音效
        audioSource.PlayOneShot(soundeffect);
        //移动乐谱

        switch (Core.selectedInstrument)
        {
            case 0:
                staffs[Core.selectedInstrument].Translate(Vector3.left * (0.32f + DistanceOffsetForBass[index[Core.selectedInstrument] - 1]) );
                break;
            
            case 1:
                staffs[Core.selectedInstrument].Translate(Vector3.left * (0.32f + DistanceOffsetForTuba[index[Core.selectedInstrument] - 1]) );
                break;
            case 2:
                staffs[Core.selectedInstrument].Translate(Vector3.left * (0.32f + DistanceOffsetForEupho[index[Core.selectedInstrument] - 1]) );
                break;
        }

      
       
       


    }
}
