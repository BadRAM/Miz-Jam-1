using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CensorBox : MonoBehaviour
{
    private bool _active; // starts true, is set to false once placed and static.
    private bool _held; // 
    private bool _censoring;
    private Box _box;
    [SerializeField] private float corner_x = -28;
    [SerializeField] private float corner_y = -17;

    private Camera _camera;
    private float startX = 0;
    private float startY = 0;
    private float endX = 0;
    private float endY = 0;
    private Vector3 _point1;
    public List<Rect> rectanglesToCensor = new List<Rect>();
    public Dither dither;
    // Start is called before the first frame update

    void Start()
    {
        _camera = Camera.main;
    }
    
    void Render()
    {

    }

    private Rect rectangle(float startX, float startY, float endX, float endY){
        float llX, llY, urX, urY = 0;
        if (startX < endX)
        {
            llX = startX;
            urX = endX;
        }
        else
        {
            llX = endX;
            urX = startX;
        }
        if (startY < endY)
        {
            llY = startY;
            urY = endY;
        }
        else
        {
            llY = endY;
            urY = startY;
        }
        return (new Rect(llX, llY, urX - llX, urY - llY));
    }
    // Update is called once per frame
    void Update()
    {
        if (_active)
        {
            if (Input.GetMouseButtonUp(0))
            {
                _censoring = true;
                _active = false;
            }
        }


        if (_censoring)
        {
            Vector3 pos = _camera.ScreenToWorldPoint(Input.mousePosition);
            if (_held)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    endX = (pos.x - corner_x + dither.lastposx) * Mathf.Pow(2 , dither.lastmipLevel) ;
                    endY = (pos.y - corner_y + dither.lastposy) * Mathf.Pow(2 , dither.lastmipLevel) ;
                    _held = false;
                    _censoring = false;
                    rectanglesToCensor.Add(rectangle(startX, startY, endX, endY));
                    Debug.Log( startX +" "+ startY +" "+ endX +" "+ endY);
                    dither.start_dither(dither.lastinput_image, dither.lastposx, dither.lastposy, dither.lastmipLevel);
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    startX = (pos.x - corner_x + dither.lastposx) * Mathf.Pow(2 , dither.lastmipLevel) ;
                    startY = (pos.y  - corner_y + dither.lastposy) * Mathf.Pow(2 , dither.lastmipLevel) ;
                    _held = true;
                }
            }
            endX = pos.x;
            endY = pos.y;
        }
    }

    public void Activate()
    {
        _active = true;

        Debug.Log("censoring");
    }

    public void Deactivate()
    {
        _active = false;
    }
}
