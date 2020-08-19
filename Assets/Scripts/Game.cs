using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    [SerializeField] private Transform mainMenu;
    [SerializeField] private Transform credits;
    
    [Header("GameScreen Links")]
    [SerializeField] private Transform gameScreen;
    [SerializeField] private GameObject message; // the prefab of the message system which tells you if you failed or succeeded the last image.
    
    [SerializeField] private StringToSprites titleText;

    [SerializeField] private Level[] levels;
    private int _currentLevel;
    private int _clues;

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
        
    }

    public void Submit()
    {
        // check if 
        if (true) // if the submitted image censored the SCPs
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
        
    }



}
