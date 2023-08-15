using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum CardClass
{
    Diamonds,
    Levels,
    Special,
}
public class HeroCardController : MonoBehaviour
{
 

    [SerializeField] int price;
    [SerializeField] bool isDiamond = false;
    
    [SerializeField] TextMeshProUGUI _textPrice;
    [SerializeField] Sprite characterSprite;
    [SerializeField] Sprite diamondSprite;
    [SerializeField] Sprite starSprite;

    [SerializeField] MoneyManager moneyManager;
    [SerializeField] string heroName;
    [SerializeField] GameObject heroPanelObject, heroShopObject;
    [SerializeField] HeroPageController heroPageController;
    Image characterImagePlaceImage, priceIconPlaceImage, frameImage;
    [SerializeField] public bool isBuy = false;

    [SerializeField] CardClass cardClass;

    void Start()
    {
        heroPanelObject.SetActive(false);
        _textPrice.text = price.ToString();
        characterImagePlaceImage = this.transform.Find("Frame/CharacterImage").GetComponent<Image>();
        frameImage = this.transform.Find("Frame").GetComponent<Image>();
        characterImagePlaceImage.sprite = characterSprite;

        if (!isBuy)
        {
            characterImagePlaceImage.color = Color.black;
        }

        priceIconPlaceImage = this.transform.Find("Frame/Price/PriceIcon").GetComponent<Image>();

        if(isDiamond)
            priceIconPlaceImage.GetComponent<Image>().sprite = diamondSprite;
        else priceIconPlaceImage.GetComponent<Image>().sprite = starSprite;
    }

    public void ChangeColorToWhite()
    {
        characterImagePlaceImage.color = Color.white;
    }

    public void SetBuyState()
    {
        isBuy = true;
        ChangeColorToWhite();
    }
   
    public void OpenHeroPage()
    {
        heroPanelObject.SetActive(true);
        heroPageController.FillInfo(this.GetComponent<HeroCardController>());
        heroShopObject.SetActive(false);

    }

    public int GetPrice()
    {
        return price;
    }
    public Sprite GetIconSprite()
    {
        return priceIconPlaceImage.GetComponent<Image>().sprite;
    }

    public Sprite GetHeroSprite()
    {
        return characterImagePlaceImage.sprite;
    }
    public Image GetHeroFrameObject()
    {
        return frameImage;
    }
    public string GetHeroName()
    {
        return heroName;
    }
    public bool GetBuyState()
    {
        return isBuy;
    }
    public bool GetIconState()
    {
        return isDiamond;
    }
    public CardClass GetCardClass()
    {
        return cardClass;
    }

}
