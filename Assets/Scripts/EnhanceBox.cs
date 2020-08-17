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
        _box.SetVisible(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_active)
        {
            Vector3 pos = _camera.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                //zoom
                Deactivate();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Deactivate();
            }
            else
            {
                int centerx = (int)Mathf.Clamp(pos.x, boxLeftBound + boxWidth, boxRightBound - boxWidth);
                int centery = (int)Mathf.Clamp(pos.y, boxBottomBound + boxHeight, boxTopBound - boxHeight);
                _box.SetBoxCoords(centery + boxHeight, centery - boxHeight, centerx - boxWidth, centerx + boxWidth);
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
