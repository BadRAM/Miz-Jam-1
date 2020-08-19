using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level", order = 1)]
public class Level : ScriptableObject
{
    public string Briefing;
    public int ClueLimit;
    public Image[] Images;
    public AudioClip MusicIntro;
    public AudioClip MusicLoop;
}
