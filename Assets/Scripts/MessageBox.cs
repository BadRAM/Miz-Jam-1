using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBox : Box
{
    [SerializeField] private StringToSprites string1;
    [SerializeField] private StringToSprites string2;
    [SerializeField] private StringToSprites string3;
    [SerializeField] private Transform StartPos;
    [SerializeField] private Transform EndPos;

    [SerializeField] private float Delay;
    [SerializeField] private float Delay2;
    public float duration = 0.5f;
    private float startTime;
    private float journeyLength;

    // Start is called before the first frame update
    void Start()
    {
        UpdateBox();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        messageManager();
    }

    void messageManager()
    {
        if (Time.time <= startTime + Delay)
        {
            showMessage();
        }
        else
        {
            hideMessage();
        }
        if (Time.time >= startTime + Delay * Delay2)
        {
            gameObject.SetActive(false);
        }
    }

    private void showMessage()
    {
        float distCovered = (Time.time - startTime) * duration;
        float fractionOfJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(StartPos.position, EndPos.position, fractionOfJourney);
        transform.position = new Vector3((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
    }

    private void hideMessage()
    {
        float distCovered = (Time.time - (startTime+Delay)) * duration;
        float fractionOfJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(EndPos.position, StartPos.position, fractionOfJourney);
        transform.position = new Vector3((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
    }

    public void activateMessage(string Line1, string Line2="")
    {
        string2.updateSprites(Line1);
        string3.updateSprites(Line2);
        gameObject.SetActive(true);
        startTime = Time.time;
        journeyLength = Vector3.Distance(StartPos.position, EndPos.position);
    }
}
