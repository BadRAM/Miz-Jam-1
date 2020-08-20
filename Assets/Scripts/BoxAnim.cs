using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxAnim : MonoBehaviour
{
    public Box box;
    [SerializeField] private GameObject dither;
    [SerializeField] private GameObject[] enhance;
    [SerializeField] private GameObject[] submit;
    [SerializeField] private GameObject[] Censor;
    [SerializeField] private StringToSprites enhanceButton;
    [SerializeField] private StringToSprites submitButton;
    [SerializeField] private StringToSprites CensorButton;
    [SerializeField] private float AnimationSpeed = 0.002f;
    [SerializeField] private int BoxTop;
    [SerializeField] private int BoxBottom;
    [SerializeField] private int BoxRight;
    [SerializeField] private int BoxLeft;
    [SerializeField] private float zLayer;
    public bool StartAnim = false;
    public bool StartReverseAnim = false;
    
    void Start()
    {
        zLayer = this.transform.position.z;
        if(box != null)
        {
            box.top = 0;
            box.bottom = 0;
            box.right = 0;
            box.left = 0;
            StartCoroutine(BoxAnimation(AnimationSpeed));
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(StartAnim)
        {
            StartCoroutine(BoxAnimation(AnimationSpeed));
        }

        if(StartReverseAnim)
        {
            StartCoroutine(BoxAnimationReverse(AnimationSpeed));
        }
    }

    // Update is called once per frame
    IEnumerator BoxAnimation(float sec)
    {
        StartAnim = false;
        box.BoxIsUpdating = true;
        transform.position = new Vector3(transform.position.x, transform.position.y, zLayer);
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
            objects.SetActive(true);
        }
        yield return new WaitForSeconds(0.75f);
        if(submit != null)
        {
            foreach(GameObject objects in submit)
            {
                objects.SetActive(true); 
            }
        }
        yield return new WaitForSeconds(0.75f);
        if(Censor != null)
        {
            foreach(GameObject objects in Censor)
            {
                objects.SetActive(true); 
            }
        }
        StopCoroutine(BoxAnimation(AnimationSpeed));
    }
    IEnumerator BoxAnimationReverse(float sec)
    {
        StartReverseAnim = false;
        box.BoxIsUpdating = true;
        if(Censor != null)
        {
            StartCoroutine(CensorButton.SpritesAnimReverse());
            yield return new WaitForSeconds(1f);
            foreach(GameObject objects in Censor)
            {
                objects.SetActive(false);
            }
        }
        if(submit != null)
        {
            StartCoroutine(submitButton.SpritesAnimReverse());
            yield return new WaitForSeconds(1f);
            foreach(GameObject objects in submit)
            {
                objects.SetActive(false);
            }
        }
        if(enhance != null)
        {
            StartCoroutine(enhanceButton.SpritesAnimReverse());
            yield return new WaitForSeconds(1f);
            foreach(GameObject objects in enhance)
            {
                objects.gameObject.SetActive(false);
            }
        }
        yield return new WaitForSeconds(0.75f);
        if(dither != null)
        {
            dither.SetActive(false);
        }
        while(box.top != 0)
        {
            yield return new WaitForSeconds(sec);
            box.top--;
        }
        while(box.bottom != 0)
        {
            yield return new WaitForSeconds(sec);
            box.bottom++;
        }
        while(box.right != 0)
        {
            yield return new WaitForSeconds(sec);
            box.right--;
        }
        while(box.left != 0)
        {
            yield return new WaitForSeconds(sec);
            box.left++;
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, 30);
        box.BoxIsUpdating = false;
        StopCoroutine(BoxAnimationReverse(AnimationSpeed));
    }
}
