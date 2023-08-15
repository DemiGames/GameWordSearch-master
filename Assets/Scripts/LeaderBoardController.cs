using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class LeaderBoardController : MonoBehaviour
{
    [SerializeField] Leaderboard leaderboard;
    [SerializeField] GameObject _entryPrefab, entriesObject;
    GameObject clone;
    [SerializeField] GameObject[] otherPlayersEntries;
    [SerializeField] GameObject playerEntry;
    float placeHeight;
    Transform scoreTextObj, nameTextObj, imagePlaceObj, textPlaceObj;
    int entriesCount;
    string userPlaceRank;
    string[] namePlacesArray, scorePlacesArray;
    void Start()
    {
        //leaderboard.UpdateLeaderBoard();
        //namePlacesArray = new string[5];
        //scorePlacesArray = new string[5];
    }

    public void ShowOnOpen()
    {
        //leaderboard.UpdateLeaderBoard();
        ////Thread.Sleep(5000);
        
        //entriesCount = leaderboard.GetEntriesCount();
        ////entriesCount = 5;
        //namePlacesArray = leaderboard.GetNameArray();
        //scorePlacesArray = leaderboard.GetScoreArray();
        //userPlaceRank = leaderboard.GetUserRank();
        ////namePlacesArray = new string[]{ "gsdgd", "SGSGA", "dgdsg", "dgdsgs", "gdsgs" };
        ////scorePlacesArray = new string[] { "5", "15456", "1545", "999999", "000" };

        //for (int i = 0; i < entriesCount; i++) 
        //{
            
        //    //clone = Instantiate(_entryPrefab, entriesObject.transform) as GameObject;
        //    //placeHeight = clone.GetComponent<RectTransform>().rect.height;
        //    // clone.transform.position = new Vector3(0, startPos.y - i * placeHeight, 0);
            
            
        //    nameTextObj = otherPlayersEntries[i].transform.Find("EntryBackground/Name");
        //    scoreTextObj = otherPlayersEntries[i].transform.Find("EntryBackground/Score");
        //    //imagePlaceObj = clone.transform.Find("EntryBackground/PlaceBack/PlaceImage");

        //    nameTextObj.GetComponent<TextMeshProUGUI>().text = namePlacesArray[i];
        //    scoreTextObj.GetComponent<TextMeshProUGUI>().text = scorePlacesArray[i];
        //    Debug.Log("Show");
        //    Debug.Log(namePlacesArray[i]);
        //    Debug.Log(scorePlacesArray[i]);
        //    //if (i < 3)
        //    //    imagePlaceObj.GetComponent<Image>().sprite = topPlacesSprites[i];
        //    //else
        //    //{
        //    //    imagePlaceObj.GetComponent<Image>().sprite. = false;
        //    //    textPlaceObj = clone.transform.Find("EntryBackground/PlaceBack/PlaceText");
        //    //    textPlaceObj.GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
        //    //    textPlaceObj.GetComponent<TextMeshProUGUI>().enabled = true;

        //    //}

        //    //clone.transform.position = anchor.transform.position - new Vector3(0, 50 * i, 0);
        //}

        //playerEntry.transform.Find("EntryBackground/Name").GetComponent<TextMeshProUGUI>().text = "ÂÛ";

        //playerEntry.transform.Find("EntryBackground/Score").
        //    GetComponent<TextMeshProUGUI>().text = Progress.Instance.playerInfo.levels.ToString();

        //playerEntry.transform.Find("EntryBackground/PlaceBack/PlaceText").GetComponent<TextMeshProUGUI>().text = userPlaceRank;

    }


}
