using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class NextLvlController : MonoBehaviour
{
    [SerializeField] StarsAnimation starsAnimation;
    [SerializeField] GameObject bottomButtons;
    [SerializeField] GameObject printingWordButtons;
    [SerializeField] LevelManager levelManager;
    [SerializeField] AdvManager advManager;


   
    public void OnCancelNextLvlPanel()
    {
        //Реклама
#if !UNITY_EDITOR
        advManager.ShowAdv();
#endif

        starsAnimation.Reset();
        bottomButtons.SetActive(true);

        if (levelManager.isClickLettersGameMode())
            printingWordButtons.SetActive(true);

        
        levelManager.LoadNewLvl();
        gameObject.SetActive(false);

    }

    public void OnShowNextLvlPanel()
    {
       
        printingWordButtons.SetActive(false);
        bottomButtons.SetActive(false);
    }
    public void OnMiddlePanel()
    {
        levelManager.ClearField();
    }
    

}
