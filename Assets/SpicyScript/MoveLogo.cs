using System.Collections;
using UnityEngine;

public class MoveLogo : MonoBehaviour
{
    [SerializeField] private float WhereToStop = 0;
    [SerializeField] private float Second = 1f;
    [SerializeField] private GameObject Startbutton;
    [SerializeField] private GameObject Stopbutton;
    private bool DoOnce = true;

    void Awake()
    {
        transform.position = new Vector2(-14.5f, 30.5f);
        if(Startbutton != null && Stopbutton != null)
        {
            Startbutton.SetActive(false);
            Stopbutton.SetActive(false);
        }
        StartCoroutine(Move(WhereToStop));
    }

    IEnumerator Move (float StopPos)
    {
        Debug.Log("Coroutine started");
        while(transform.position.y != StopPos)
        {
            yield return new WaitForSeconds(Second);
            transform.position = new Vector2(transform.position.x, transform.position.y - 1f);
        }
        yield return new WaitForSeconds(0.3f);
        if(Startbutton != null && Stopbutton != null)
        {
            Startbutton.SetActive(true);
            Stopbutton.SetActive(true);
        }
        StopCoroutine(Move(WhereToStop));
        Debug.Log("Coroutine stopped");
    }
}
