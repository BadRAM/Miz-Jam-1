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
        if (_active)
        {
            Vector3 pos = _camera.ScreenToWorldPoint(Input.mousePosition);
            int centerX = (int)Mathf.Clamp(pos.x, boxLeftBound + boxWidth, boxRightBound - boxWidth);
            int centerY = (int)Mathf.Clamp(pos.y, boxBottomBound + boxHeight, boxTopBound - boxHeight);
            _box.SetBoxCoords(centerY + boxHeight, centerY - boxHeight, centerX - boxWidth, centerX + boxWidth);

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 topLeft = new Vector3(
                    Mathf.Clamp01((pos.x - boxLeftBound) / (boxRightBound - boxLeftBound)),
                    Mathf.Clamp01((pos.y - boxBottomBound) / (boxTopBound - boxBottomBound)), 0);
                
                Debug.Log(topLeft);
                _dither.ZoomIn(topLeft);
                
                Deactivate();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Deactivate();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(1))
            {
                _dither.ZoomOut();
            }
        }
    }

    public void Deactivate()
    {
        _active = false;
        _box.SetVisible(false);
    }

    public void Activate()
    {
        _active = true;
        _box.SetVisible(true);
    }
}
