using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageBox : Box
{
    [SerializeField] private StringToSprites string1;
    [SerializeField] private StringToSprites string2;
    [SerializeField] private StringToSprites string3;
    [SerializeField] private Transform StartPos;
    [SerializeField] private Transform EndPos;

    [SerializeField] AudioSource goodFeedback;
    [SerializeField] AudioSource badFeedback;
    [SerializeField] AudioSource neutralFeedback;

    [SerializeField] private float Delay;
    [SerializeField] private float persist;
    public float duration = 0.5f;
    private float startTime;
    private float journeyLength;
    private int _successLevel;
    private bool _soundPlaying;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time - (Delay + persist);
        UpdateBox();
    }

    // Update is called once per frame
    void Update()
    {
        messageManager();
    }

    void messageManager()
    {
        if (Time.time >= startTime + Delay)
        {
            if (Time.time >= startTime + Delay + persist - duration)
            {
                hideMessage(false);
            }
            else
            {
                if (!_soundPlaying)
                {
                    determineSFX(_successLevel);
                    _soundPlaying = true;
                }
                showMessage();
            }
        }
        else
        {
            hideMessage(true);
        }
    }

    private void showMessage()
    {
        float distCovered = (Time.time - (startTime + Delay) / duration);
        transform.position = Vector3.Lerp(StartPos.position, EndPos.position, distCovered);
        transform.position = new Vector3((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
    }

    private void hideMessage(bool instant)
    {
        if (instant)
        {
            transform.position = StartPos.position;
            return;
        }
        float distCovered = Mathf.Clamp01(Time.time - (startTime + Delay + (persist - duration))) / duration;
        transform.position = Vector3.Lerp(EndPos.position, StartPos.position, distCovered);
        transform.position = new Vector3((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
    }

    public void activateMessage(int successLevel)
    {
        Debug.Log(successLevel);

        _soundPlaying = false;
        _successLevel = successLevel;
        
        switch (successLevel)
        {
            case 0:
                string2.updateSprites("Hazards not censored.");
                string3.updateSprites("Amnestics applied.");
                break;
            case 1:
                string2.updateSprites("Hazards censored.");
                string3.updateSprites("no progress made.");
                break;
            case 2:
                string2.updateSprites("Hazards censored.");
                string3.updateSprites("clue discovered.");
                break;
        }
        gameObject.SetActive(true);
        startTime = Time.time;
        journeyLength = Vector3.Distance(StartPos.position, EndPos.position);
    }

    private void determineSFX(int i)
    {
        if(i == 0)
        {
            badFeedback.Play();
        }
        else if(i == 1)
        {
            neutralFeedback.Play();
        }
        else
        {
            goodFeedback.Play();
        }
    }
}
