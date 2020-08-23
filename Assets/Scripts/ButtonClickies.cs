using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClickies : MonoBehaviour
{
    public AudioSource click1;
    public AudioSource click2;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            click1.Play();
        }

        if (Input.GetMouseButtonDown(1))
        {
            click2.Play();
        }
    }
}
