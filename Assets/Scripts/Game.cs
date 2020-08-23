﻿using System;
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
    [SerializeField] private GameObject message; // the prefab of the message system which tells you if you failed or succeeded the last image.
    [SerializeField] private BoxAnim boxAnim;
    
    [SerializeField] private StringToSprites titleText;

    [SerializeField] private Level[] levels;
    private int _currentLevel;
    private int _currentImage;
    private int _clues;
    [SerializeField] private Dither dither;

    private void Start()
    {
        mainMenu.gameObject.SetActive(true);
        gameScreen.gameObject.SetActive(false);
        credits.gameObject.SetActive(false);
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
        _currentImage = 0;
        dither.currentImage = levels[_currentLevel].Images[_currentImage];
    }

    public void Submit()
    {
        bool imageCheck = true; // is true if all hazards censored.
        bool pointCheck;
        foreach (var i in dither.currentImage.Hazards)
        {
            pointCheck = true;
            foreach (var l in dither.censor.rectanglesToCensor)
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
            float percentOfImageCovered = 0.5f;
            if (Random.value < percentOfImageCovered) // if the submitted image didn't censor too much of the rest of the image.
            {
                _clues++;
            }

            if (_clues == levels[_currentLevel].ClueLimit)
            {
                FinishLevel();
            }
            else
            {
                Instantiate(message);
                //message.GetComponent<Message>().SetCorrect(true);
            }
        }
        else
        {
            Instantiate(message);
            //message.GetComponent<Message>().SetCorrect(false);
        }
    }

    public void FinishLevel()
    {
        if (_currentLevel == levels.Length)
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
