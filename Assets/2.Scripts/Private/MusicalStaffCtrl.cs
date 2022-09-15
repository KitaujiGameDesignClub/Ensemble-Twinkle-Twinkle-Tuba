using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalStaffCtrl : MonoBehaviour,IUpdate
{
   
    



        [Header("三个乐器")]
    public GameObject[] instruments = new GameObject[3];
    [Header("三个乐谱")]
    public CursorCtrl[] staffs = new CursorCtrl[3];

    [Header("三个指法显示")] 
    public GameObject[] fingeringShow = new GameObject[3];
    
    
   

   
    
    [Header("辅助线 光标")] 
    public Transform Cursor;
    




    // Start is called before the first frame update
    void Start()
    {
        //先禁用乐器和乐谱
        for (int i = 0; i < 3; i++)
        {
            instruments[i].SetActive(false);
            staffs[i].gameObject.SetActive(false);
            fingeringShow[i].gameObject.SetActive(false);
        }
        //禁用判定线
        Cursor.gameObject.SetActive(false);
     
        
        //播放视频
        StaticVideoPlayer.staticVideoPlayer.Play();
      
        //注册update
        UpdateManager.RegisterUpdate(this);

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

        for (int i = 0; i < 3; i++)
        {
            if(i == Core.core.selectedInstrument)
            {
                //显示乐器
                instruments[Core.core.selectedInstrument].SetActive(true);
                //显示乐谱
                staffs[Core.core.selectedInstrument].gameObject.SetActive(true);
                fingeringShow[Core.core.selectedInstrument].SetActive(true);
            }
            else
            {
                //卸载不需要的乐器和乐谱
             Destroy(instruments[i]);   
             Destroy(staffs[i].gameObject);
             Destroy(fingeringShow[i]);
            }
        }
        
     
      //显示判定线
       Cursor.gameObject.SetActive(true); 
    
    
    //一段时间之后注册事件，让乐谱移动
 //  Invoke(nameof(register),startTimeOffset);
    }

    /// <summary>
    /// 注册，让乐谱移动
    /// </summary>
    void register()
    {
        StaticVideoPlayer.staticVideoPlayer.RegisterEachFrame();
        StaticVideoPlayer.staticVideoPlayer.eachFrame.AddListener(delegate
        {
            //注册让乐谱移动
            staffs[Core.core.selectedInstrument].StaffRefresh(Cursor);
           
        });
    }
    
    

    public void FastUpdate()
    {

          //跳过前面的片段
        if (Core.core.episode == 0 && Input.GetKeyDown(KeyCode.X))
        {
            StaticVideoPlayer.staticVideoPlayer.Jump(Episode.ShowStaffAndInstrument -1);
        }
        
        //跳过后面的片段
        if (Core.core.episode == 3 && Input.GetKeyDown(KeyCode.X) )
        {
            StaticVideoPlayer.staticVideoPlayer.Jump(Episode.VideoEnd - 1);
        }

        
      
        
      
        
        
     
        
        switch (StaticVideoPlayer.staticVideoPlayer.Frame)
        {
            //显示乐器和乐谱，读取本地时间文件
            case >= Episode.ShowStaffAndInstrument when Core.core.episode == 0:
               
                Core.core.episode++;
                staffs[Core.core.selectedInstrument]. SetInterval();
                //计算乐谱移动速度
                staffs[Core.core.selectedInstrument].SetSpeed(Cursor);
                showStaffAndInstrument();
                break;
            
            //乐谱开始移动
            case >= Episode.StartMoving when Core.core.episode == 1:
                Core.core.episode++;
                //注册，让乐谱移动
                register();
            break;
            
            //合奏结束
            case  >= Episode.EnsembleEnd when Core.core.episode == 2:
                Core.core.episode++;
                StaticVideoPlayer.staticVideoPlayer.eachFrame.RemoveAllListeners();
            
                //删掉乐器和乐谱
               Destroy(instruments[Core.core.selectedInstrument]); 
                Destroy( staffs[Core.core.selectedInstrument].gameObject);
             Destroy(fingeringShow[Core.core.selectedInstrument]);
                
                //删掉判定线
                Destroy(Cursor.gameObject); 
                break;
            
            //视频结束，进入小剧场
            case >= Episode.VideoEnd when Core.core.episode == 3:
                Core.core.episode++;
                Core.core.ShowDialogue();
               UpdateManager.Remove(this);
                break;
            
        }
    }

   
}
