using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAnimationController : MonoBehaviour
{
    
    private GameObject panelToIn, panelToOff;
    [SerializeField] private Canvas EnterCanvas, ExitCanvas;
    Animator panelToInAnimator, panelToOffAnimator;
    public bool isCloseCanvas;
    [SerializeField] SoundManager soundManager;
    void Start()
    {
        isCloseCanvas = true;
        //panelToInAnimator = panelToIn.GetComponent<Animator>();
        //panelToOffAnimator = panelToOff.GetComponent<Animator>();
        
    }
    
    public void StartSwitchPanel()
    {
        //panelToOffAnimator.SetBool("isPanelOff", true);
        
        //float animDuration = panelToOffAnimator.GetCurrentAnimatorStateInfo(0).length;
        
        //StartCoroutine(WaitBeforeInactive(animDuration));
                
    }
    IEnumerator WaitBeforeInactive(float duration)
    {             
        yield return new WaitForSeconds(duration);
        if(isCloseCanvas)
            ExitCanvas.gameObject.SetActive(false);
        EnterCanvas.gameObject.SetActive(true);
        
    }

    public void SwitchCanvas()
    {
        soundManager.MakeSwapPanelSound();
        ExitCanvas.gameObject.SetActive(false);
        EnterCanvas.gameObject.SetActive(true);
    }


}
