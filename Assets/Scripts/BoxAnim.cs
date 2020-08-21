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
    [SerializeField] private Transform LogoHideBlackBar;
    [SerializeField] private StringToSprites[] Texts;
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
            box.top = BoxTop - (BoxTop - BoxBottom)/2;
            box.bottom = BoxBottom + (BoxTop - BoxBottom)/2;
            box.right = 0;
            box.left = 0;
            box.zLayer = 20;
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
    public IEnumerator BoxAnimation(float sec)
    {
        StartAnim = false;
        box.BoxIsUpdating = true;
        while(LogoHideBlackBar.position.y > 2)
        {
            yield return new WaitForSeconds(0.1f);
            LogoHideBlackBar.position = new Vector3(LogoHideBlackBar.position.x, LogoHideBlackBar.position.y -1, LogoHideBlackBar.position.z);
        }
        box.zLayer = zLayer;
        foreach(StringToSprites text in Texts)
        {
            text._TextPlay = true;
        }
        while(box.right != BoxRight)
        {
            yield return new WaitForSeconds(sec);
            box.right++;
            box.left--;
        }
        box.left = BoxLeft;
        
        while(box.top != BoxTop)
        {
            yield return new WaitForSeconds(sec);
            box.top++;
            box.bottom--;
        }
        box.bottom = BoxBottom;
        
        box.UpdateBox();
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
    public IEnumerator BoxAnimationReverse(float sec)
    {
        StartReverseAnim = false;
        box.BoxIsUpdating = true;
        foreach(StringToSprites text in Texts)
        {
            text._TextPlayReverse = true;
        }
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
            box.bottom++;
        }
        box.bottom = 0;
        
        while(box.right != 0)
        {
            yield return new WaitForSeconds(sec);
            box.right--;
            box.left++;
        }
        box.left = 0;
        
        
        while(LogoHideBlackBar.position.y < 12)
        {
            yield return new WaitForSeconds(0.1f);
            LogoHideBlackBar.position = new Vector3(LogoHideBlackBar.position.x, LogoHideBlackBar.position.y + 1, LogoHideBlackBar.position.z);
        }
        box.zLayer = zLayer;
        box.UpdateBox();
        box.BoxIsUpdating = false;
        StopCoroutine(BoxAnimationReverse(AnimationSpeed));
    }
}
