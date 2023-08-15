using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource m_AudioSource;
    [SerializeField] AudioClip chooseSound, unchooseSound, swapPanelsSound, winRoundSound, guessWordSound, wrongWordSound;


    float soundVolume;
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        soundVolume = m_AudioSource.volume;
        SetVolume();
    }

    void SetVolume()
    {
        if (Progress.Instance.playerInfo.isMute)
            m_AudioSource.volume = 0;


    }
    public void switchSound()
    {        
        if (m_AudioSource.volume > 0f)
            m_AudioSource.volume = 0f;
 
        else
            m_AudioSource.volume = soundVolume;

        Progress.Instance.playerInfo.isMute = !Progress.Instance.playerInfo.isMute;
        Progress.Instance.Save();
    }


    public void MakeChooseSquareSound()
    {
        m_AudioSource.clip = chooseSound;
        Play();
    }

    public void MakeUnchooseSquareSound()
    {
        m_AudioSource.clip = unchooseSound;
        Play();
    }

    public void MakeSwapPanelSound()
    {
        m_AudioSource.clip = swapPanelsSound;
        Play();
    }


    public void MakeWinRoundSound()
    {
        m_AudioSource.clip = winRoundSound;
        Play();
    }

    public void MakeGuessingWordSound()
    {
        
        m_AudioSource.clip = guessWordSound;
        Play();
    }

    public void MakeWrongWordSound()
    {
        m_AudioSource.clip = wrongWordSound;
        Play();
    }
    
    void Play()
    {
        m_AudioSource.Play();
    }

}
