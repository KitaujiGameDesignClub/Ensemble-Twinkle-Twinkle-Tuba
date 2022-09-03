using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    public GameObject[] instruments = new GameObject[3];

    public GameObject[] staffs = new GameObject[3];

    private void Awake()
    {
        for (int i = 0; i < 3; i++)
        {
            instruments[i].SetActive(false);
//            staffs[i].SetActive(false);
        }
    }

    public void StartGame(int id)
    {
        //显示乐器，应该延迟到视频到开始合奏之前的一小段时间
        DemonstrateInstrument(id);
    }

    private void DemonstrateInstrument(int id)
    {
        instruments[id].SetActive(true);
      //  staffs [characterId].SetActive(true);
    }
    
    // Start is called before the first frame update
     void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
