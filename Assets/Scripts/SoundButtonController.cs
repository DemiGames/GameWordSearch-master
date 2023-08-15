using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundButtonController : MonoBehaviour
{
    [SerializeField] Sprite soundOffImage, soundOnImage;
    void Start()
    {
        soundOnImage = GetComponent<Image>().sprite;
        SetImage();
    }


    public void SetImage()
    {
        if (Progress.Instance.playerInfo.isMute)
            GetComponent<Image>().sprite = soundOffImage;
    }

    public void switchSprite()
    {

        if (Progress.Instance.playerInfo.isMute)
        {
            GetComponent<Image>().sprite = soundOffImage;
            return;
        }

        GetComponent<Image>().sprite = soundOnImage;

    }

}
