using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Controller : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    private Image squareImage;
    [SerializeField] GameObject frameObject;
    
    [SerializeField] private Animator animator;
    SquaresManager squaresManager;

    [SerializeField] SoundManager soundManager;
    
    
    public bool isChoosed;
    private bool isBlocked;
    bool isHint;
    public bool isPrev;
    private void Start()
    {       
        squaresManager = FindObjectOfType<SquaresManager>();      
        animator = GetComponent<Animator>();
        squareImage = GetComponentInChildren<Image>();
        
        isChoosed = false;
        isBlocked = true;
        isPrev = false;
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();

    }
    //
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        
        if (!isChoosed && !isBlocked)
        {
            squaresManager.StartWord(gameObject);
            AttemptSquare();
        }
        
    }

    //При клике на квадрат
    public void OnPointerEnter(PointerEventData pointerEventData)
    {       
        
        if (squaresManager.GetMouseDragging())      
        {
            
            
            if (!isChoosed && !isBlocked)
            {
                if (squaresManager.CanCheckSquare(gameObject))
                {
                    AttemptSquare();
                }
            }
            
            else if(isChoosed && !isBlocked)
            {
                squaresManager.ClearLastSquare(gameObject);
            }
        }

    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        squaresManager.EndWord(gameObject);
    }

    public void AttemptSquare()
    {
        
        isChoosed = true;
        isHint = true;
        squareImage.color = squaresManager.MakeAttemptColor();
        animator.SetBool("isAttempt", true);
        
        
        soundManager.MakeChooseSquareSound();
        squaresManager.AddSquareToList(gameObject);
        
    }

    public void UnChooseSquare()
    {
        isChoosed = false;
        isHint = false;
        animator.SetBool("isAttempt", false);
        
        squareImage.color = squaresManager.MakeOriginalColor();
        

        soundManager.MakeUnchooseSquareSound();
    }
    public void FinalSquare()
    {
        isBlocked = true;
        isChoosed = false;
        isHint = true;
        squareImage.color = squaresManager.MakeSuccesColor();
        animator.SetBool("isAttempt", false);
        
        animator.SetBool("isGuessRight", true);
        frameObject.SetActive(false);
    }


    public bool isHintSquare()
    {
        return isHint;
    }

    public bool isChoosenSquare()
    {
        return isChoosed;
    }

    public void HintSquare()
    {
        isBlocked = false;
        isChoosed = false;
        isHint = true;
        frameObject.SetActive(true);
    }

    public void ClearHintSquare()
    {
        isHint = false;
        frameObject.SetActive(false);
    }
    public void ClearSquare()
    {
        isBlocked = false;
        isChoosed = false;
        isHint = false;
        animator.SetBool("isGuessRight", false);
        animator.SetBool("isAttempt", false);
        //animator.SetBool("isWrong", false);
        ClearHintSquare();
        squareImage.color = squaresManager.MakeOriginalColor();

    }

    public void StartWrongSquareAnim()
    {
        isChoosed = false;
        isHint = false;
        StartCoroutine(WrongWordAnimation());

    }
    private IEnumerator WrongWordAnimation()
    {        
        float speed = 0.25f;      
        float timeElapsed = 0;
        squareImage.color = squaresManager.MakeWrongColor();
        while (timeElapsed <= speed)
        {
            squareImage.color = Color.Lerp(squaresManager.MakeWrongColor(), squaresManager.MakeOriginalColor(), timeElapsed / speed);

            timeElapsed += Time.deltaTime;
            
            yield return null;
        }
        animator.SetBool("isAttempt", false);
        squareImage.color = squaresManager.MakeOriginalColor();

    }

    public void LockSquare(bool lockMode)
    {
        isBlocked = lockMode;
    }

}
