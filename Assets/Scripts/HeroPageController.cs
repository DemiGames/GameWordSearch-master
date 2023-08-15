using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class HeroPageController : MonoBehaviour
{
    int price;
    string heroName;
    Sprite characterSprite, iconSprite;
    [TextArea(5, 6)]
    [SerializeField] public string levelDescription;
    [TextArea(5, 6)]
    [SerializeField] public string diamondsDescription;
    [TextArea(5, 6)]
    [SerializeField] public string specialDescription;

    [SerializeField] TextMeshProUGUI _textDescription;
    [SerializeField] TextMeshProUGUI _textName;
    [SerializeField] MoneyManager moneyManager;

    [SerializeField] GameObject characterImagePlace, priceIconImagePlace;
    [SerializeField] GameObject buyButton, heroShopObject;
    HeroCardController heroCardController;
    [SerializeField] HeroListController heroListController;
    CardClass heroClass;
    void Start()
    {
        _textName.enabled = heroCardController.isBuy;
    }

    public void FillInfo(HeroCardController _heroCardController)
    {
        heroCardController = _heroCardController;
        price = heroCardController.GetPrice();
        iconSprite = heroCardController.GetIconSprite();
        characterSprite = heroCardController.GetHeroSprite();
        heroName = heroCardController.GetHeroName();
        heroClass = heroCardController.GetCardClass();

        levelDescription = $"Пройдите {price} уровней";
        diamondsDescription = $"Открыть за {price} кристаллов";
        specialDescription = "Откройте всех персонажей";

        _textName.text = heroName;
        
        characterImagePlace.GetComponent<Image>().sprite = characterSprite;
        priceIconImagePlace.GetComponent<Image>().sprite = iconSprite;

        switch (heroClass)
        {
            case CardClass.Diamonds:
                _textDescription.text = diamondsDescription;
                break;
            case CardClass.Levels:
                _textDescription.text = levelDescription;
                break;
            case CardClass.Special:
                _textDescription.text = specialDescription;
                priceIconImagePlace.GetComponent<Image>().enabled = false;
                break;
        }
        if (heroCardController.GetBuyState())
        {
            UnlockHero();
            SetColorToWhite();
        }
        else
            LockHero();
        
    }

    public void OnUnclock()
    {
        if (heroClass == CardClass.Diamonds && Progress.Instance.playerInfo.money < price)
        {
            return;
        }
        if (heroClass == CardClass.Levels && Progress.Instance.playerInfo.levels < price)
        {
            return;
        }
        if (heroClass == CardClass.Special)
        {
            int counter = 0;
            foreach (var item in Progress.Instance.playerInfo.isHeroBuyArr)
            {
                if (item) counter++;
            }
            if(counter < Progress.Instance.playerInfo.isHeroBuyArr.Length - 1) 
            {
                return;
            }
        }
            
        StartCoroutine(OpenAnimation());
        
        if(heroClass == CardClass.Diamonds)
            moneyManager.SpendForOpenCharacter(price);

        heroCardController.SetBuyState();
        heroListController.SaveBuyState(heroCardController);

        UnlockHero();



    }
    private void UnlockHero()
    {
        _textName.enabled = heroCardController.isBuy;
        _textDescription.enabled = false;
        buyButton.SetActive(false);
        priceIconImagePlace.SetActive(false);
    }

    public void SetColorToWhite()
    {      
        characterImagePlace.GetComponent<Image>().color = Color.white;
    }

    private void LockHero()
    {
        characterImagePlace.GetComponent<Image>().color = Color.black;
        _textName.enabled = false;
        _textDescription.enabled = true;
        buyButton.SetActive(true);
        priceIconImagePlace.SetActive(true);
    }
    //По кнопке закрытия
    public void CloseHeroPagePanel()
    {
        gameObject.SetActive(false);
        heroShopObject.SetActive(true);
        priceIconImagePlace.GetComponent<Image>().enabled = true;
    }

    private IEnumerator OpenAnimation()
    {
        float speed = 0.5f;
        float timeElapsed = 0;
        while (timeElapsed <= speed)
        {
            characterImagePlace.GetComponent<Image>().color = Color.Lerp(Color.black, Color.white, timeElapsed / speed);

            timeElapsed += Time.deltaTime;

            yield return null;
        }
    }
}
