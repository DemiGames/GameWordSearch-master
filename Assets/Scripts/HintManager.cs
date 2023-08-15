using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HintManager : MonoBehaviour
{
    [SerializeField] SquaresManager squaresManager;
    [SerializeField] LevelManager levelManager;
    [SerializeField] MoneyManager moneyManager;
    [SerializeField] TextMeshProUGUI hintCountText;
    [SerializeField] Canvas HintsShopCanvas;
    [SerializeField] int hintCount = 1;
    [SerializeField] int hintPrice = 3;
    [SerializeField] int hintsForAds = 3;

    [SerializeField] RewardedAvdButtonController rewardedAvdButtonController;

    [SerializeField] TextMeshProUGUI AlertTextObject;


    [DllImport("__Internal")]
    private static extern void ShowRewardedAdvExtern();

    void Start()
    {
        AlertTextObject.gameObject.SetActive(false);
        hintCount = Progress.Instance.playerInfo.hints;
        UpdateHintCountText();        
    }
    public void GiveHint()
    {
        if (hintCount == 0)
        {
            HintsShopCanvas.gameObject.SetActive(true);
            return;
        }
        string hintWord = levelManager.GetFirstWordToHint();
        
        if (squaresManager.isWordGuessedCheck())
        {
            squaresManager.HintWord(hintWord);
            hintCount -= 1;
            UpdateHintCount();
            UpdateHintCountText();
        }
    }
    void UpdateHintCount()
    {
        Progress.Instance.playerInfo.hints = hintCount;
        Progress.Instance.Save();
    }
    void UpdateHintCountText()
    {
        hintCountText.text = hintCount.ToString();
    }   
    public void BuyingHint()
    {
        //Если недостаточно денег на покупку подсказки
        if (moneyManager.GetMoneyCount() < hintPrice)
        {
            AlertTextObject.gameObject.SetActive(true);
            return;
        }

        hintCount += 1;
        moneyManager.ChangeMoneyCount(-hintPrice);

        UpdateHintCount();
        UpdateHintCountText();       
    }
    //По кнопке
    public void ShowAdForReward()
    {
        if (rewardedAvdButtonController.GetAccess())
        {
            hintCount += hintsForAds;

#if !UNITY_EDITOR
            ShowRewardedAdvExtern();
#endif
        }

    }
    //Подсказки за просмотр рекламы(в jslib)
    public void HintsForWatchAds()
    {        
        UpdateHintCount();
        UpdateHintCountText();       
    }
   public void CloseHintsCanvas()
    {
        HintsShopCanvas.gameObject.SetActive(false);
    }
 
}
