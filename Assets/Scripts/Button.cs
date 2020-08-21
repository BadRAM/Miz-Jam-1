using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private int IndexOfNextScene;
    [SerializeField] private AudioClip _ButtonIsHoveredOver;
    [SerializeField] private AudioClip _ButtonIsPressed;
    [SerializeField] private AudioSource _AS;

    public UnityEvent Clicked;
    public List<SpriteRenderer> Sprites = new List<SpriteRenderer>();
    
    private void OnMouseEnter() 
    {
        foreach(SpriteRenderer Sprite in Sprites)
        {
            if(Sprites != null)
            {
                Sprite.color = Color.grey;
                if(_AS != null)
                {
                    _AS.PlayOneShot(_ButtonIsHoveredOver);
                }
            }
        }
    }

    private void OnMouseExit() 
    {
        foreach(SpriteRenderer Sprite in Sprites)
        {
            Sprite.color = Color.white;
        }
    }
    private void OnMouseDown()
    {
        foreach(SpriteRenderer Sprite in Sprites)
        {
            Sprite.color = Color.red;
            if(_AS != null)
            {
                _AS.PlayOneShot(_ButtonIsPressed);
            }
        }
        Clicked.Invoke();
    }

    private void OnMouseUp() 
    {        
        foreach(SpriteRenderer Sprite in Sprites)
        {
            Sprite.color = Color.grey;
        }
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(IndexOfNextScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
