using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TexTest : MonoBehaviour
{
    [SerializeField] private Texture2D tex;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(tex.width);
        Debug.Log(tex.mipmapCount + "Mip map levels");
        Debug.Log((float)tex.GetPixels(0).Length / (float)tex.GetPixels(1).Length);
        Debug.Log((float)tex.GetPixels(0).Length / (float)tex.GetPixels(2).Length);
        Debug.Log((float)tex.GetPixels(1).Length / (float)tex.GetPixels(2).Length);
        Debug.Log(tex.GetPixels(0).Length);
        Debug.Log(tex.GetPixels(1).Length);
        Debug.Log(tex.GetPixels(2).Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
