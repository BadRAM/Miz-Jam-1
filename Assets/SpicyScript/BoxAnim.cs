using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxAnim : MonoBehaviour
{
    public Box box;
    [SerializeField] private GameObject dither;
    [SerializeField] private GameObject[] enhance;
    [SerializeField] private GameObject submit;
    [SerializeField] private GameObject Censor;
    [SerializeField] private float AnimationSpeed = 0.002f;
    [SerializeField] private int BoxTop;
    [SerializeField] private int BoxBottom;
    [SerializeField] private int BoxRight;
    [SerializeField] private int BoxLeft;
    
    void Start()
    {
        if(box != null)
        {
            box.top = 0;
            box.bottom = 0;
            box.right = 0;
            box.left = 0;
            StartCoroutine(BoxAnimation(AnimationSpeed));
        }
    }

    // Update is called once per frame
    IEnumerator BoxAnimation(float sec)
    {
        while(box.top != BoxTop)
        {
            yield return new WaitForSeconds(sec);
            box.top++;
        }
        while(box.bottom != BoxBottom)
        {
            yield return new WaitForSeconds(sec);
            box.bottom--;
        }
        while(box.right != BoxRight)
        {
            yield return new WaitForSeconds(sec);
            box.right++;
        }
        while(box.left != BoxLeft)
        {
            yield return new WaitForSeconds(sec);
            box.left--;
        }
        box.BoxIsUpdating = false;
        yield return new WaitForSeconds(sec);
        if(dither != null)
        {
            dither.SetActive(true);
        }
        yield return new WaitForSeconds(2f);
        foreach(GameObject objects in enhance)
        {
            objects.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(0.75f);
        if(submit != null)
        {
            submit.SetActive(true);
        }
        yield return new WaitForSeconds(0.75f);
        if(Censor != null)
        {
            Censor.SetActive(true);
        }
        StopCoroutine(BoxAnimation(AnimationSpeed));
    }
}
