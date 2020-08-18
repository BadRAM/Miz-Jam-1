using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] Sprites;
    [SerializeField] private int IndexOfNextScene;

    public UnityEvent Clicked;
    
    private void OnMouseEnter() 
    {
        foreach(SpriteRenderer Sprite in Sprites)
        {
            Sprite.color = Color.grey;
        }
    }

    private void OnMouseExit() 
    {
        foreach(SpriteRenderer Sprite in Sprites)
        {
            Sprite.color = Color.white;
        }
    }
    void OnMouseDown()
    {
        foreach(SpriteRenderer Sprite in Sprites)
        {
            Sprite.color = Color.red;
        }
        Clicked.Invoke();
    }

    void OnMouseUp() 
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
