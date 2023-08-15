using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardedAvdButtonController : MonoBehaviour
{
    //Animator animator;
    //Button button;
    bool isAllowedToRewardAdv = true;
    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponent<Animator>();
        //button = GetComponent<Button>();
        isAllowedToRewardAdv = true;
    }
    private void OnEnable()
    {
        
        if(!isAllowedToRewardAdv) 
        {            
            DisableButton();
        }
    }

    public bool GetAccess()
    {
        return isAllowedToRewardAdv;
    }
    public void DisableButton()
    {
        isAllowedToRewardAdv = false;
    }

    public void ActiveButton()
    {
        isAllowedToRewardAdv = true;
    }
}
