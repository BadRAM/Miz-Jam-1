using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    [SerializeField] private Transform mainMenu;
    [SerializeField] private Transform credits;
    
    [Header("GameScreen Links")]
    [SerializeField] private Transform gameScreen;
    [SerializeField] private MessageBox message; // the prefab of the message system which tells you if you failed or succeeded the last image.
    [SerializeField] private BoxAnim boxAnim;
    
    [SerializeField] private StringToSprites titleText;
    [SerializeField] private StringToSprites note;

    [SerializeField] private AudioSource submitSound;

    [SerializeField] private Level menuMusicLevel;
    [SerializeField] private Level[] levels;
    private int _currentLevel;
    private int _currentImage;
    private int _clues;
    private MusicMan _musicMan;
    [SerializeField] private Dither dither;

    private void Start()
    {
        mainMenu.gameObject.SetActive(true);
        gameScreen.gameObject.SetActive(false);
        credits.gameObject.SetActive(false);

        _musicMan = GetComponentInChildren<MusicMan>();
        _musicMan.UpdateLevel(menuMusicLevel);
    }

    public void BeginGame()
    {
        mainMenu.gameObject.SetActive(false);
        gameScreen.gameObject.SetActive(true);
        credits.gameObject.SetActive(false);

        _currentLevel = 0;

        LevelInit();
    }

    private void LevelInit()
    {
        boxAnim.StartAnim = true;
        note.deleteChildren();
        note.TextToConvert = levels[_currentLevel].Note;
        note.CreateSprites();
        
        _musicMan.UpdateLevel(levels[_currentLevel]);


        _clues = 0;
        _currentImage = 0;
        dither.currentImage = levels[_currentLevel].Images[_currentImage];
        dither.restart_dither();
        dither.censor.rectanglesToCensor.RemoveRange(0, dither.censor.rectanglesToCensor.Count);
    }

    public void Submit()
    {
        submitSound.Play();
        bool imageCheck = true; // is true if all hazards censored.
        bool pointCheck;
        foreach (Vector2 i in dither.currentImage.Hazards)
        {
            pointCheck = true;
            foreach (Rect l in dither.censor.rectanglesToCensor)
            {
                if (l.Contains(i))
                {
                    pointCheck = false;
                }
            }

            if (pointCheck == true)
            {
                imageCheck = false;
            }
        }
        
        
        if (imageCheck) // if the submitted image censored the SCPs
        {
            float area = 0;
            foreach (Rect i in dither.censor.rectanglesToCensor)
            {
                area += i.height * i.width;
            }

            int successLevel = 1;
            float rand = Random.value;
            
            Debug.Log("comparing " + rand + " to " + area / 107520);
            if (Random.value > area / 107520) // if the submitted image didn't censor too much of the rest of the image.
            {
                _clues++;
                successLevel = 2;
            }

            if (_clues == levels[_currentLevel].ClueLimit)
            {
                FinishLevel();
            }
            else
            {
                message.activateMessage(successLevel);
                NextImage();
            }
        }
        else
        {
            message.activateMessage(0);
            NextImage();
        }
    }

    void NextImage()
    {
        _currentImage++;
        dither.currentImage = levels[_currentLevel].Images[_currentImage];
        dither.restart_dither();
        dither.censor.rectanglesToCensor.RemoveRange(0, dither.censor.rectanglesToCensor.Count);
    }

    public void FinishLevel()
    {
        if (_currentLevel == levels.Length-1)
        {
            FinishGame();
            return;
        }

        _currentLevel++;
        
        LevelInit();
    }

    public void FinishGame()
    {
        mainMenu.gameObject.SetActive(false);
        gameScreen.gameObject.SetActive(false);
        credits.gameObject.SetActive(true);
    }
}
