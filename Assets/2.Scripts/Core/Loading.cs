using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class Loading : MonoBehaviour
{
    // Start is called before the first frame update
    private IEnumerator Start()
    {
        Settings.SaveSettings();
        yield return Resources.UnloadUnusedAssets();
        GC.Collect();

    
        yield return SceneManager.LoadSceneAsync("SampleScene");
    }
}