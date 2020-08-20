using System.Collections;
using UnityEngine.Audio;
using UnityEngine;

public class MoveLogo : MonoBehaviour
{
    [SerializeField] private float WhereToStop = 0;
    [SerializeField] private float Second = 1f;
    [SerializeField] private GameObject Startbutton;
    [SerializeField] private GameObject Stopbutton;
    [SerializeField] private AudioClip _TheLogoWhenMoving;
    [SerializeField] private AudioClip _ButtonsAppearing;
    [SerializeField] private AudioSource _SourceThis;
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
        while(transform.position.y != StopPos)
        {
            yield return new WaitForSeconds(Second);
            transform.position = new Vector2(transform.position.x, transform.position.y - 1f);
            if(_SourceThis != null)
            {
                _SourceThis.PlayOneShot(_TheLogoWhenMoving);
            }
        }
        yield return new WaitForSeconds(0.3f);
        if(Startbutton != null && Stopbutton != null)
        {
            Startbutton.SetActive(true);
            Stopbutton.SetActive(true);
            if(_SourceThis != null)
            {
                _SourceThis.PlayOneShot(_ButtonsAppearing);
            }
        }
        StopCoroutine(Move(WhereToStop));
    }
}
