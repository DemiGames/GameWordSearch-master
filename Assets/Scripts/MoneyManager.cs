using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] int moneyCount = 1;
    [SerializeField] int moneyForLvl = 5;
    int moneyForGamerate = 50;
    int tempTextMoney;
    [SerializeField] float addMoneyAnimationTime;
    [SerializeField] AnimationCurve animationCurve;
    
    void Start()
    {
        moneyCount = Progress.Instance.playerInfo.money;
        moneyText.text = moneyCount.ToString();
        addMoneyAnimationTime = 0.6f;
    }

    public void AddMoneyForCompleteLvl()
    {      
        ChangeMoneyCount(moneyForLvl);     
    }
    void UpdateMoneyCount()
    {
        Progress.Instance.playerInfo.money = moneyCount;
        Progress.Instance.Save();
    }
    //в jslib
    public void AddMoneyForGamerate()
    {
        moneyCount += moneyForGamerate;
        ChangeMoneyCount(moneyForGamerate);
        //UpdateMoneyCount();
    }

    public void SpendForOpenCharacter(int price)
    {
        ChangeMoneyCount(-price);
    }
    IEnumerator UpdateMoneyAnimation(int difference)        //Попробовить без коррутины
    {
        float speed = 1 / addMoneyAnimationTime;
        float timeElapsed = 0f;
        int tempMoney = 0;
        int stepMoney = tempMoney;

        Color32 originalColor = new Color32(255,255,255,255);
        Color32 redColor = new Color32(200, 29, 13, 255);
        Color32 greenColor = new Color32(45, 219, 75, 255);

        if (difference > 0)
            moneyText.color = greenColor;
        else if(difference < 0)
            moneyText.color = redColor;

        while (timeElapsed < 1f)
        {
            timeElapsed += speed * Time.deltaTime;
            tempMoney = (int)(animationCurve.Evaluate(timeElapsed) * difference);
            
            if (stepMoney != tempMoney)
            {
                moneyText.text = (moneyCount + tempMoney).ToString();
                stepMoney = tempMoney;
            }
            yield return null;
        }
        
        moneyCount += tempMoney;
        moneyText.color = originalColor;
        UpdateMoneyCount();
    }
    public int GetMoneyCount()
    {
        return moneyCount;
    }

    public void ChangeMoneyCount(int difference)
    {
        StartCoroutine(UpdateMoneyAnimation(difference));            
    }


}
