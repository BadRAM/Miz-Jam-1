using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Censor : MonoBehaviour
{
    public GameObject censorBox;
    // Start is called before the first frame update
    void Start()
    {
        Rect rect = new Rect(0, 0, 10, 10);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(Input.mousePosition);
            Instantiate(censorBox);
            
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Debug.Log(Input.mousePosition);
        }
    }
}
