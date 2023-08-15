using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartController : MonoBehaviour
{
    [SerializeField] private Canvas GameUI, MainMenu, IngameSettings, ModeChoosing, HintShopCanvas, HeroShopCanvas;
    [SerializeField] private GameObject levelManager, moneyManager, hintManager, leaderboardObject, nextLvlObject, rateGameObject,
        wordAlertObject;

    //[SerializeField] private Progress progress;
 
    void Start()
    {
        GameUI.gameObject.SetActive(true);
        ModeChoosing.gameObject.SetActive(false);
        IngameSettings.gameObject.SetActive(false);
        HintShopCanvas.gameObject.SetActive(false);
        HeroShopCanvas.gameObject.SetActive(false);
        leaderboardObject.SetActive(false);
        nextLvlObject.SetActive(false);
        rateGameObject.SetActive(false);
        wordAlertObject.SetActive(false);

        levelManager.SetActive(true);
        moneyManager.SetActive(true);
        hintManager.SetActive(true);
        //GameUI.gameObject.SetActive(false);
        MainMenu.gameObject.SetActive(true);      
    }

    public void TurnOnGameUI()
    {
        GameUI.gameObject.SetActive(false);
    }
    
}
