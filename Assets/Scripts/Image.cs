using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Image", menuName = "Image", order = 1)]
public class Image : ScriptableObject
{
    public Texture2D Texture;
    public Vector2[] Hazards;
}
