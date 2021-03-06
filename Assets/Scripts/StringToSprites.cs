﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class StringToSprites : MonoBehaviour
{
    [SerializeField] private string textToConvert;
    [SerializeField] private List<CharSprite> spriteMap;
    [SerializeField] private GameObject spritePrefab;
    [SerializeField] private float charWidth = 1f;
    [SerializeField] private float animationInterval = 0.1f;
    [SerializeField] private Boolean Test = false;
    private Dictionary<char, Sprite> characterSpriteDict;
    [SerializeField] private List<GameObject> sprites = new List<GameObject>();
    public bool _TextPlay;
    public bool _StartWithNoText = false;
    public bool _TextPlayReverse = false;

    //public Button _button;

    void Start()
    {
        characterSpriteDict = initiateDict();
        if(_StartWithNoText)
        {
            deleteChildren();
        }
        // CreateSprites();
    }
    public string TextToConvert
    {
        get { return textToConvert; }
        set { textToConvert = value; }
    }
    void Update()
    {

        if(_TextPlay == true)
        {
            StartCoroutine(SpritesAnim());
        }

        if(_TextPlayReverse == true)
        {
            //StartCoroutine(SpritesAnimReverse());
        }
    }

    public void CreateSprites()
    {

        Dictionary<char, Sprite> charSpriteDict = initiateDict();
        deleteChildren();

        textToConvert = textToConvert.ToUpper();

        Vector3 cursorPos = new Vector3();
        foreach (char character in textToConvert)
        {
            if (character == '~')
            {
                cursorPos = new Vector3(0, cursorPos.y - 1, cursorPos.z);
                continue;
            }
            GameObject spriteObject = Instantiate(spritePrefab, transform);
            spriteObject.transform.localPosition = cursorPos;
            if (charSpriteDict.ContainsKey(character))
            {
                spriteObject.GetComponent<SpriteRenderer>().sprite = charSpriteDict[character];
            }
            cursorPos += Vector3.right * charWidth;
        }
    }

    public void deleteChildren()
    {
        int i = 0;
        GameObject[] allChildren = new GameObject[transform.childCount];
        foreach (Transform child in transform)
        {
            allChildren[i] = child.gameObject;
            i += 1;
        }
        foreach (GameObject child in allChildren){
            DestroyImmediate(child.gameObject);
        }
    }

    private Dictionary<char, Sprite> initiateDict()
    {
        Dictionary<char, Sprite> Dict = new Dictionary<char, Sprite>();
        foreach (CharSprite cs in spriteMap)
        {
            Dict.Add(cs.character, cs.sprite);
        }
        return Dict;
    }

    public void updateSprites(string updatedMessage)
    {
        textToConvert = updatedMessage;
        CreateSprites();
    }

    public IEnumerator SpritesAnim()
    {
        _TextPlay = false;
        Dictionary<char, Sprite> charSpriteDict = initiateDict();
        deleteChildren();
        textToConvert = textToConvert.ToUpper();
        Vector3 cursorPos = new Vector3();
        foreach (char character in textToConvert)
        {
            if (character == '~')
            {
                cursorPos = new Vector3(0, cursorPos.y - 1, cursorPos.z);
                continue;
            }
            yield return new WaitForSeconds(animationInterval);
            GameObject spriteObject = Instantiate(spritePrefab, transform);
            spriteObject.transform.localPosition = cursorPos;
            
            if (charSpriteDict.ContainsKey(character))
            {
                spriteObject.GetComponent<SpriteRenderer>().sprite = charSpriteDict[character];
            }
            cursorPos += Vector3.right * charWidth;
        }
        StopCoroutine(SpritesAnim());
    }
    public IEnumerator SpritesAnimReverse()
    {
        _TextPlayReverse = false;
        int i = 0;
        GameObject[] allChildren = new GameObject[transform.childCount];
        foreach (Transform child in transform)
        {
            allChildren[i] = child.gameObject;
            i += 1;
        }
        foreach (Transform child in transform)
        {
            yield return new WaitForSeconds(animationInterval);
            Destroy(child.gameObject);
        }
        if(transform.childCount != 0)
        {
            _TextPlayReverse = true;
        }
        else
        {
            StopCoroutine(SpritesAnimReverse());
        }
    }
}

[Serializable]
public struct CharSprite {
    public char character;
    public Sprite sprite;
}

#if UNITY_EDITOR
[CustomEditor(typeof(StringToSprites))]
class SurfaceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        StringToSprites stringToSprites = (StringToSprites)target;
        if (GUILayout.Button("Create Sprites"))
        {
            stringToSprites.CreateSprites();
        }
        DrawDefaultInspector();
    }
}

#endif

