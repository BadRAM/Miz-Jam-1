using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StringToSprites : MonoBehaviour
{
    [SerializeField] private string textToConvert;
    [SerializeField] private List<CharSprite> spriteMap;
    [SerializeField] private GameObject spritePrefab;
    [SerializeField] private float charWidth = 1f;

    public void CreateSprites()
    {
        Dictionary<char, Sprite> charSpriteDict = new Dictionary<char, Sprite>();
        foreach (CharSprite cs in spriteMap)
        {
            charSpriteDict.Add(cs.character, cs.sprite);
        }

        foreach (Transform child in transform) {
            DestroyImmediate(child.gameObject);
        }
        
        textToConvert = textToConvert.ToUpper();
        
        Vector3 cursorPos = new Vector3();
        foreach (char character in textToConvert)
        {
            GameObject spriteObject = Instantiate(spritePrefab, transform);
            spriteObject.transform.localPosition = cursorPos;
            if (charSpriteDict.ContainsKey(character))
            {
                spriteObject.GetComponent<SpriteRenderer>().sprite = charSpriteDict[character];
            }
            cursorPos += Vector3.right * charWidth;
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
