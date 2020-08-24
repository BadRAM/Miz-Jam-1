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
    [SerializeField] private Box topBlackBox;
    [SerializeField] private Box bottomBlackBox;
    //[SerializeField] private Transform LogoHideBlackBar;
    [SerializeField] private StringToSprites[] Texts;
    [SerializeField] private float AnimationSpeed = 0.002f;
    [SerializeField] private int BoxTop;
    [SerializeField] private int BoxBottom;
    [SerializeField] private int BoxRight;
    [SerializeField] private int BoxLeft;
    [SerializeField] private float zLayer;
    public bool StartAnim = false;
    public bool StartReverseAnim = false;
    private bool _stopreverse;
    private Game _game;
    
    void Start()
    {
        _game = FindObjectOfType<Game>();
        zLayer = this.transform.position.z;
        if(box != null)
        {
            box.top = BoxTop - (BoxTop - BoxBottom)/2;
            topBlackBox.bottom = box.top;
            topBlackBox.UpdateBox();
            box.bottom = BoxBottom + (BoxTop - BoxBottom)/2;
            bottomBlackBox.top = box.bottom;
            bottomBlackBox.UpdateBox();
            box.right = 0;
            box.left = 0;
            box.zLayer = 20;
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
            _stopreverse = true;
        }

        if(StartReverseAnim)
        {
            _stopreverse = false;
            StartCoroutine(BoxAnimationReverse(AnimationSpeed/2));
        }
    }

    // Update is called once per frame
    public IEnumerator BoxAnimation(float sec)
    {
        StartAnim = false;
        box.BoxIsUpdating = true;
        box.zLayer = zLayer;
        while(box.right != BoxRight)
        {
            yield return new WaitForSeconds(sec);
            box.right++;
            box.left--;
        }
        box.left = BoxLeft;
        
        while(box.top != BoxTop)
        {
            yield return new WaitForSeconds(sec*2);
            box.top++;
            box.bottom--;
            topBlackBox.bottom = box.top;
            topBlackBox.UpdateBox();
            bottomBlackBox.top = box.bottom;
            bottomBlackBox.UpdateBox();
        }
        box.bottom = BoxBottom;
        bottomBlackBox.top = box.bottom;
        bottomBlackBox.UpdateBox();

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
        enhanceButton.deleteChildren();
        enhanceButton._TextPlay = true;
        
        if(submit != null)
        {
            foreach(GameObject objects in submit)
            {
                objects.SetActive(true); 
            }
        }
        submitButton.deleteChildren();
        submitButton._TextPlay = true;
        
        if(Censor != null)
        {
            foreach(GameObject objects in Censor)
            {
                objects.SetActive(true); 
            }
        }
        CensorButton.deleteChildren();
        CensorButton._TextPlay = true;
        
        foreach(StringToSprites text in Texts)
        {
            text.deleteChildren();
            text._TextPlay = true;
        }
        StopCoroutine(BoxAnimation(AnimationSpeed));
    }
    public IEnumerator BoxAnimationReverse(float sec)
    {
        if (_stopreverse)
        {
            StopCoroutine(BoxAnimationReverse(AnimationSpeed));
            _stopreverse = false;
        }
        StartReverseAnim = false;
        box.BoxIsUpdating = true;
        if(Censor != null)
        {
            foreach(GameObject objects in Censor)
            {
                objects.SetActive(false);
            }
        }
        if(submit != null)
        {
            foreach(GameObject objects in submit)
            {
                objects.SetActive(false);
            }
        }
        if(enhance != null)
        {
            foreach(GameObject objects in enhance)
            {
                objects.gameObject.SetActive(false);
            }
        }
        if(dither != null)
        {
            dither.SetActive(false);
        }
        while(box.top != BoxTop - (BoxTop - BoxBottom)/2)
        {
            yield return new WaitForSeconds(sec);
            box.top--;
            box.bottom++;
            topBlackBox.bottom = box.top;
            topBlackBox.UpdateBox();
            bottomBlackBox.top = box.bottom;
            bottomBlackBox.UpdateBox();
        }
        box.bottom = BoxBottom + (BoxTop - BoxBottom)/2;
        bottomBlackBox.top = box.bottom;
        bottomBlackBox.UpdateBox();
        
        while(box.right != 0)
        {
            yield return new WaitForSeconds(sec/2);
            box.right--;
            box.left++;
        }
        box.left = 0;
        
        box.zLayer = zLayer;
        box.UpdateBox();
        box.BoxIsUpdating = false;
        
        StopCoroutine(BoxAnimationReverse(AnimationSpeed));
    }
}
