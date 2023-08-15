using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeroListController : MonoBehaviour
{
    [SerializeField] List<HeroCardController> heroCards;
    [SerializeField] Color diamondHeroColor;
    [SerializeField] Color levelHeroColor;
    [SerializeField] Color specialHeroColor;
    Color tempColor;
    void Start()
    {
        heroCards.AddRange(GetComponentsInChildren<HeroCardController>());
        for (int i = 0; i < heroCards.Count; i++)
        {
            switch (heroCards[i].GetCardClass())
            {        
                case CardClass.Diamonds:
                    tempColor = diamondHeroColor;   
                    break;
                case CardClass.Levels:
                    tempColor = levelHeroColor;
                    break;
                case CardClass.Special:
                    tempColor = specialHeroColor;
                    break;
                default:
                    break;
            }
            heroCards[i].GetHeroFrameObject().color = tempColor;

            if (Progress.Instance.playerInfo.isHeroBuyArr[i])
                heroCards[i].SetBuyState();
        }

    }
    public void SaveBuyState(HeroCardController _heroCardController)
    {
        int index = heroCards.IndexOf(_heroCardController);
        Progress.Instance.playerInfo.isHeroBuyArr[index] = true;
        Progress.Instance.Save();
    }

}
