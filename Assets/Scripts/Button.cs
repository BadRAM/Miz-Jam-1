using System;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private Color hover = Color.gray;
    [SerializeField] private Color press = Color.red;
    [SerializeField] private bool checkbox;

    public UnityEvent Clicked;
    public List<SpriteRenderer> Sprites = new List<SpriteRenderer>();

    public bool checkboxState;

    private void Start()
    {
        foreach(SpriteRenderer Sprite in Sprites)
        {
            if (checkbox)
            {
                if (checkboxState)
                {
                    Sprite.color = Color.white;
                }
                else
                {
                    Sprite.color = press;
                }
            }
            else
            {
                Sprite.color = Color.white;
            }
        }
    }

    private void OnMouseEnter() 
    {
        foreach(SpriteRenderer Sprite in Sprites)
        {
            if(Sprites != null)
            {
                Sprite.color = hover;
            }
        }
    }

    private void OnMouseExit() 
    {
        foreach(SpriteRenderer Sprite in Sprites)
        {
            if (checkbox)
            {
                if (checkboxState)
                {
                    Sprite.color = Color.white;
                }
                else
                {
                    Sprite.color = press;
                }
            }
            else
            {
                Sprite.color = Color.white;
            }
        }
    }
    private void OnMouseDown()
    {
        foreach(SpriteRenderer Sprite in Sprites)
        {
            Sprite.color = press;

        }
        Clicked.Invoke();

        if (checkbox)
        {
            checkboxState = !checkboxState;
        }
    }

    private void OnMouseUp() 
    {        
        foreach(SpriteRenderer Sprite in Sprites)
        {
            Sprite.color = hover;
        }
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
