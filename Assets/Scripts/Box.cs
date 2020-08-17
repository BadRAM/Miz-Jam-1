using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private Transform spriteNW;
    [SerializeField] private Transform spriteN;
    [SerializeField] private Transform spriteNE;
    [SerializeField] private Transform spriteW;
    [SerializeField] private Transform spriteE;
    [SerializeField] private Transform spriteSW;
    [SerializeField] private Transform spriteS;
    [SerializeField] private Transform spriteSE;
    [SerializeField] private Transform spriteCenter;

    [SerializeField] private int top;
    [SerializeField] private int bottom;
    [SerializeField] private int left;
    [SerializeField] private int right;

    [SerializeField] private float zLayer;

    // Start is called before the first frame update
    void Start()
    {

    }


    private void OnValidate()
    {
        UpdateBox();
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateBox();
    }

    public void SetBoxCoords(int Top, int Bottom, int Left, int Right)
    {
        top = Top;
        bottom = Bottom;
        left = Left;
        right = Right;
        UpdateBox();
    }

    private void UpdateBox()
    {
        spriteNW.position = new Vector3(left, top, zLayer);
        spriteN.position = new Vector3(right + (left - right)*0.5f, top, zLayer);
        spriteN.GetComponent<SpriteRenderer>().size = new Vector2(1, (right - left - 1));
        spriteNE.position = new Vector3(right, top, zLayer);
        spriteE.position = new Vector3(right, top + (bottom - top)*0.5f, zLayer);
        spriteE.GetComponent<SpriteRenderer>().size = new Vector2(1, (top - bottom - 1));
        spriteW.position = new Vector3(left, top + (bottom - top)*0.5f, zLayer);
        spriteW.GetComponent<SpriteRenderer>().size = new Vector2(1, (top - bottom - 1));
        spriteSW.position = new Vector3(left, bottom, zLayer);
        spriteS.position = new Vector3(right + (left - right)*0.5f, bottom, zLayer);
        spriteS.GetComponent<SpriteRenderer>().size = new Vector2(1, (right - left - 1));
        spriteSE.position = new Vector3(right, bottom, zLayer);
    }

    public void SetVisible(bool visible)
    {
        spriteNW.GetComponent<SpriteRenderer>().enabled = visible;
        spriteN.GetComponent<SpriteRenderer>().enabled = visible;
        spriteNE.GetComponent<SpriteRenderer>().enabled = visible;
        spriteE.GetComponent<SpriteRenderer>().enabled = visible;
        spriteW.GetComponent<SpriteRenderer>().enabled = visible;
        spriteSW.GetComponent<SpriteRenderer>().enabled = visible;
        spriteSE.GetComponent<SpriteRenderer>().enabled = visible;
        spriteS.GetComponent<SpriteRenderer>().enabled = visible;
        spriteCenter.GetComponent<SpriteRenderer>().enabled = visible;

    }
}
