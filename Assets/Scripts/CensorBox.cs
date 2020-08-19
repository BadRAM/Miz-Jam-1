using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CensorBox : MonoBehaviour
{
<<<<<<< HEAD
=======

    private bool _active; // starts true, is set to false once placed and static.
    private bool _held; // 
    private Box _box;

    private Vector3 _point1;
    
    
>>>>>>> e2dfe6595fd5f0d342c94e3663380ee2bb9b88a4
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
        
=======
        if (_active)
        {
            if (!_held)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _held = true;
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(1))
                {
                    _held = false;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    _held = false;
                    
                }
            }
        }
    }

    public void Activate()
    {
        _active = true;
    }

    public void Deactivate()
    {
        _active = false;
>>>>>>> e2dfe6595fd5f0d342c94e3663380ee2bb9b88a4
    }
}
