using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private List<string> wordsList = new List<string>();
    List<string> copiedWordsList;


    [SerializeField] private TextMeshProUGUI[] wordsPlaces;
    [SerializeField] private GameObject printingWordSys;
    
    [SerializeField] private GameObject NextLvlPanel;
    [SerializeField] private TextMeshProUGUI LevelText;

    [SerializeField] private SquaresManager squaresManager;   
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private RateGameController rateGameController;
    [SerializeField] WordsDictionary wordsDictionary;

    [SerializeField] private List<string> LevelWordsList;
    [SerializeField] RewardedAvdButtonController rewardedAvdButtonController;
    private List<string> copiedLevelWordsList = new List<string>();
    private TextMeshProUGUI printingWord;

    string guessedWord;
    TextMeshProUGUI guessingWordObject;
    public int countWordsToNext = 4;
    int wordIndex = 0;

    int countGuessedWords = 0;
    int countLevels = 0;
    int levelsForRategame = 10;

    
    
    [HideInInspector] public Animator wordAnimator;
    bool isClickLettersMode = false;

    [SerializeField] SoundManager soundManager;

    [DllImport("__Internal")]
    private static extern void SetToLeaderboard(int value);

    void Start()
    {
        wordsList = wordsDictionary.GetWordsList();
        copiedWordsList = new(wordsList);
        printingWordSys.gameObject.SetActive(true);
        printingWord = printingWordSys.GetComponentInChildren<TextMeshProUGUI>();
        countLevels = Progress.Instance.playerInfo.levels;
        LevelText.text = countLevels.ToString();
    }
    //Очитска последней введённой буквы слова
    public void DeleteLastLetter()
    {
        
        int countLetters = printingWord.text.Length;
        if (countLetters > 1)
        {
            printingWord.text = printingWord.text.Remove(countLetters - 2, 1);
            squaresManager.ClearLastLetter(); 
        }

    }
    //Очитска набранного слова
    public void DeleteAllLetters()
    {
        int countLetters = printingWord.text.Length;
        for (int i = 0; i < countLetters; i++)
        {
            DeleteLastLetter();
        }
    }

    //Получение режима от пользователя
    public void SetClickLettersGameMode(bool mode)
    {
        
        isClickLettersMode = mode;
        if (isClickLettersMode)
        {
            printingWordSys.gameObject.SetActive(true);            
        }
        else
        {           
            printingWordSys.gameObject.SetActive(false);
            
        }       
        LoadNewLvl();
    }

    public bool isClickLettersGameMode()
    {        
        return isClickLettersMode;
    }
    public void ShowWord(string collectedWord)
    {
        printingWord.text = collectedWord + "_";
    }
    public void RecieveGuessedWord(string guessWord)
    { 
        guessedWord = guessWord;
        SearchAndOut(guessedWord);
        ++countGuessedWords;

        if (isClickLettersMode)
            printingWord.text = "_";

        if (countGuessedWords == countWordsToNext)
        {
            StartCoroutine(ShowNextLvlPanel());               
        }
    }

    private IEnumerator ShowNextLvlPanel()
    {              
                 
        yield return new WaitForSeconds(1.2f);
        //Оценка игры
        if (countLevels == levelsForRategame)
        {
            rateGameController.gameObject.SetActive(true);
        }

        Animator nxtLvlAnimator = NextLvlPanel.GetComponent<Animator>();
        NextLvlPanel.SetActive(true);
        nxtLvlAnimator.SetBool("isNextLvl", true);
        soundManager.MakeWinRoundSound();
    }

    public void hideRateGameWindow()
    {
        rateGameController.gameObject.SetActive(false);
    }

    public void AddLevelCount()
    {
        countLevels++;        
        LevelText.text = countLevels.ToString();

        Progress.Instance.playerInfo.levels = countLevels;
        Progress.Instance.Save();

#if !UNITY_EDITOR
        SetToLeaderboard(Progress.Instance.playerInfo.levels); 
#endif
    }

    //Вызывается по кнопке
    public void LoadNewLvl()
    {
        squaresManager.LockAllSquares(true);
        countGuessedWords = 0;
        printingWord.text = "_";
        ChooseWordsAndPlace();
        copiedLevelWordsList = new List<string>(LevelWordsList);

        rewardedAvdButtonController.ActiveButton();
        foreach (var place in wordsPlaces)
        {
            wordAnimator = place.GetComponent<Animator>();
            wordAnimator.SetBool("isGuessed", false);
            place.fontStyle = FontStyles.Normal;
        }
        
        squaresManager.GenerateAllLetters();
        StartCoroutine(squaresManager.GetComponent<SquaresManager>().ShowAllLettersOnField());
        
        
    }
  
    public void ClearField()
    {
        squaresManager.ClearField();
    }

    private void SearchAndOut(string Word)
    {
        wordIndex = LevelWordsList.IndexOf(Word);
        guessingWordObject = wordsPlaces[wordIndex];
        wordAnimator = guessingWordObject.GetComponent<Animator>();
        
        
        copiedLevelWordsList.Remove(Word);
    }

    public void WordLogic()
    {
        //Анимация слова сверху
        guessingWordObject.GetComponent<GuessingWord>().StartWordAnimate();
        soundManager.MakeGuessingWordSound();
    }

    public void StrikethrougWord()
    {
        wordsPlaces[wordIndex].fontStyle = FontStyles.Strikethrough;
    }

    public List<string> GetLevelWords()
    {
        return LevelWordsList;
    }

    public string GetFirstWordToHint()
    {     
        return copiedLevelWordsList[0];
    }
    private List<string> SelectRandomWords(List<string> themeWordList, int count)
    {
        List<string> selectedWords = new List<string>();
        // Проверяем, что в списке достаточно слов для выбора
        if (count > themeWordList.Count)
        {
            return selectedWords;
        }

        for (int i = 0; i < count; i++)
        {
            string selectedWord = SelectRandomWord(themeWordList);
            
            selectedWords.Add(selectedWord);
            themeWordList.Remove(selectedWord);
        }

        return selectedWords;
    }

    public string SelectRandomWord(List<string> words)
    {
        int randomIndex = UnityEngine.Random.Range(0, words.Count);
        string randomWord = words[randomIndex];
        return randomWord;
    }

    private void ChooseWordsAndPlace()
    {
        LevelWordsList = SelectRandomWords(copiedWordsList, countWordsToNext);
        StartCoroutine(ShowWords());
    }
    IEnumerator ShowWords()
    {
        Animator wordAnimator;
        int i = 0;
        foreach (var place in wordsPlaces)
        {
            wordAnimator = place.GetComponent<Animator>();
            place.text = LevelWordsList[i];
            wordAnimator.SetBool("isEnter", true);
            i++;
            yield return new WaitForSeconds(1f/(wordsPlaces.Length - 1));

        }
    }
}
    

