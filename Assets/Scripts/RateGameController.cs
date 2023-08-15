using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class RateGameController : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void RateGame();

    private void Start()
    {
       
    }
    public void showRateGameWindow()
    {
#if !UNITY_EDITOR
            RateGame();                     
#endif
    }

   

}
