using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

public class GuessingWord : MonoBehaviour
{
    TextMeshProUGUI _textObject;
    string wordText;
    [SerializeField] float delayBetweenLetters = 0.005f;           //Время между анимацией    
    [SerializeField] Color targetColor;
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        _textObject = GetComponent<TextMeshProUGUI>();
    }

    public void StartWordAnimate()
    {       
        StartCoroutine(AnimateTextColor());
    }
    private IEnumerator AnimateTextColor()
    {
        int totalCharacters = _textObject.text.Length;
        string formatLetter = "";
        wordText = _textObject.text;
        string formatText = "";
        string endOfWord = wordText;
        

        for (int i = 0; i < totalCharacters; i++)
        {
            endOfWord = endOfWord.Substring(1);

            formatLetter = "<color=#" + ColorUtility.ToHtmlStringRGBA(targetColor) + ">" + "<s>" + wordText[i] + "</s>" + "</color>";

            formatText += formatLetter;

            _textObject.text = formatText + endOfWord;
            // Добавляем задержку перед переходом к следующей букве
            yield return new WaitForSeconds(delayBetweenLetters/totalCharacters);
        }
    }
}

