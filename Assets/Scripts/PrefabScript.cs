using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[System.Serializable]
public class Prefabs
{
   
    public  Transform prefabTransform;
    public TMP_Text textWord;
    public  SpriteRenderer spriteRend;
}
public class PrefabScript : MonoBehaviour
{
    
    public  Prefabs Prefabs;
    public static PrefabScript prefabs;
}
