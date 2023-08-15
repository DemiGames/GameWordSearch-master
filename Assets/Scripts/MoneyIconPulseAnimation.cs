using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MoneyIconPulseAnimation : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void StartPulse()
    {
        animator.SetBool("isPulse", true);
    }
    public void EndPulse()
    {
        animator.SetBool("isPulse", false);
    }
 

 
}
