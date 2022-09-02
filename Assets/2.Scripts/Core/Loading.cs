using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
