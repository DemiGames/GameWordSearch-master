using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TestScript : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Старт");
        image = GetComponentInChildren<Image>();
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        //Output to console the GameObject's name and the following message
        Debug.Log("Вошёл " + name + " GameObject");
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        //Output the name of the GameObject that is being clicked
        Debug.Log(name + " Зажал");
        image.color = Color.red;
    }

    //Detect if clicks are no longer registering
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        Debug.Log(name + "Отжал");
    }
}
