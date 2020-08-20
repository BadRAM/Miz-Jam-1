using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnhanceBox : MonoBehaviour
{
    [SerializeField] private int boxWidth;
    [SerializeField] private int boxHeight;
    [SerializeField] private int boxTopBound;
    [SerializeField] private int boxBottomBound;
    [SerializeField] private int boxLeftBound;
    [SerializeField] private int boxRightBound;
    
    private bool _active;
    private Box _box;
    private Camera _camera;
    private Dither _dither;
    private int _zoomLevel;

    // Start is called before the first frame update
    void Start()
    {
        _box = GetComponent<Box>();
        _camera = Camera.main;
        _dither = FindObjectOfType<Dither>();
        _box.SetVisible(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(_dither == null)
        {
            _dither = FindObjectOfType<Dither>();
        }
        
        if (_active)
        {
            Vector3 pos = _camera.ScreenToWorldPoint(Input.mousePosition);

            int centerX = (int)Mathf.Clamp(pos.x, boxLeftBound + boxWidth, boxRightBound - boxWidth);
            int centerY = (int)Mathf.Clamp(pos.y, boxBottomBound + boxHeight, boxTopBound - boxHeight);
            _box.SetBoxCoords(centerY + boxHeight, centerY - boxHeight, centerX - boxWidth, centerX + boxWidth);

            if (Input.GetMouseButtonDown(0))
            {
                if (pos.y < boxTopBound && pos.y > boxBottomBound && pos.x < boxRightBound && pos.x > boxLeftBound)
                {
                    Vector3 topLeft = new Vector3(
                        Mathf.Clamp01((pos.x - (boxLeftBound + boxWidth)) / ((boxRightBound - boxWidth) - (boxLeftBound + boxWidth))),
                        Mathf.Clamp01((pos.y - (boxBottomBound + boxHeight)) / ((boxTopBound - boxHeight) - (boxBottomBound + boxHeight))), 0);
                
                    Debug.Log(pos);
                    _dither.ZoomIn(topLeft);
                    _zoomLevel = _dither.GetZoom();
                }
                Deactivate();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Deactivate();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(1) && _zoomLevel != 0)
            {
                _dither.ZoomOut();
                _zoomLevel = _dither.GetZoom();
            }
        }
    }

    public void Cancel()
    {
        if (_active)
        {
            Deactivate();
        }
        else
        {
            _dither.ZoomOut();
            _zoomLevel = _dither.GetZoom();
        }
    }

    public void Deactivate()
    {
        _active = false;
        _box.SetVisible(false);
    }

    public void Activate()
    {
        if (_zoomLevel == 2)
        {
            return;
        }
        _active = true;
        _box.SetVisible(true);
    }
}
