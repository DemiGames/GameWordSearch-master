using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LvlIconPulseAnimation : MonoBehaviour
{
    Animator animator;
    [SerializeField] TextMeshProUGUI lvlText;
    private void Start()
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

    public void AddLvl()
    {

        
    }
}
