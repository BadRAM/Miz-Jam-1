using System;
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
    [SerializeField] private Boolean Test = false;
    private Dictionary<char, Sprite> characterSpriteDict;

    void Start()
    {
        characterSpriteDict = initiateDict();
        CreateSprites();
    }

    public string TextToConvert
    {
        get { return textToConvert; }
        set { textToConvert = value; }
    }

    public void CreateSprites()
    {

        Dictionary<char, Sprite> charSpriteDict = initiateDict();
        deleteChildren();

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
            deleteChildren();
            updatedMessage = updatedMessage.ToUpper();

            Vector3 cursorPos = new Vector3();
            foreach(char character in updatedMessage)
            {
                GameObject spriteObject = Instantiate(spritePrefab, transform);
                spriteObject.transform.localPosition = cursorPos;
                if (characterSpriteDict.ContainsKey(character))
                {
                    spriteObject.GetComponent<SpriteRenderer>().sprite = characterSpriteDict[character];
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

