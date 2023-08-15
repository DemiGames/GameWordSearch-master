using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertTextController : MonoBehaviour
{
    
    void Start()
    {
        
    }

    public void OnEndAnimations()
    {
        gameObject.SetActive(false);
    }
}
