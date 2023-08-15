using System.Runtime.InteropServices;
using UnityEngine;


[System.Serializable]
public class PlayerInfo
{
    public int money = 1;
    public int levels = 1;
    public int hints = 1;
    public bool isMute = false;
    public bool[] isHeroBuyArr = new bool[14];
}


public class Progress : MonoBehaviour
{
    public PlayerInfo playerInfo;
    [DllImport("__Internal")]
    private static extern void SaveExtern(string date);

    [DllImport("__Internal")]
    private static extern void LoadExtern();

 

    public static Progress Instance;


    private void Awake()
    {
        if(Instance == null)
        {
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
            Instance = this;

#if !UNITY_EDITOR
            LoadExtern();                    
#endif

        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Save()
    {
        string jsonString = JsonUtility.ToJson(playerInfo);

#if !UNITY_EDITOR
        SaveExtern(jsonString);
#endif
    }

    public void SetPlayerInfo(string value)
    {        
        playerInfo = JsonUtility.FromJson<PlayerInfo>(value);        
    }
    public void ClearProgress()
    {
        playerInfo = new PlayerInfo();
    }
    //private void Update()
    //{
    //    if (Input.GetKeyUp(KeyCode.Backspace))
    //    {
    //        ClearProgress();
    //        Progress.Instance.Save();
    //    }

    //}


}


