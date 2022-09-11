using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomImage : MonoBehaviour
{
    public Sprite[] images;
    
    // Start is called before the first frame update
    void Start()
    {
      var s = GetComponent<Image>();
      s.sprite = images[Random.Range(0, images.Length)];
      s.color = new Color(1f, 1f, 1f, 0.6f);
    }

    // Update is called once per frame
  
}
