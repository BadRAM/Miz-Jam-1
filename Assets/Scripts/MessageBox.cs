using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBox : Box
{
    [SerializeField] private StringToSprites string1;
    [SerializeField] private StringToSprites string2;
    [SerializeField] private float speed;
    private bool transition = false;

    // Start is called before the first frame update
    void Start()
    {
        UpdateBox();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transition = true;
        }
        if (transition)
        {
            showMessage("That was horrible");
        }
        
    }

    private void showMessage(string message)
    {
        //default position = 17.5
        transition = true;
        if(transform.position.y > 17.5)
        {
            string2.updateSprites(message);
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
        else
        {
            transition = false;
        }
    }
}
