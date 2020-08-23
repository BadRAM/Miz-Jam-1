using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMan : MonoBehaviour
{
    public Level currentLevel;

    private AudioSource _audioSource;

    public void UpdateLevel(Level newLevel)
    {
        currentLevel = newLevel;
        if (currentLevel.MusicIntro != null)
        {
            _audioSource.loop = false;
            _audioSource.clip = currentLevel.MusicIntro;
            _audioSource.Play();
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.clip = currentLevel.MusicLoop;
            _audioSource.Play();
            _audioSource.loop = true;
        }
    }
}
