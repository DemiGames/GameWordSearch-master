using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class AdvManager : MonoBehaviour
{
    float advTimer;
    float advBreak = 61f;

    [DllImport("__Internal")]
    private static extern void ShowIntersitialAdvExtern();

    private void Start()
    {
        advTimer = advBreak;
    }
    private void Update()
    {
        advTimer -= Time.deltaTime;
    }

    public void ShowAdv()
    {
        if (advTimer <= 0)
        {
#if !UNITY_EDITOR
            ShowIntersitialAdvExtern();
#endif
        }
    }

    public void StartTimer()
    {
        advTimer = advBreak;
    }
}
