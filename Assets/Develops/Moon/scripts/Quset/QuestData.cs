using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "QuestData", menuName = "Scriptable Object/QuestData")]
public class QuestData : ScriptableObject
{
    [SerializeField] public string questtitle;
    [SerializeField] public int value;
    [SerializeField] public bool isclear;
    [SerializeField] public string monster;
    [SerializeField] public string item;
    [SerializeField] public int clearValue;
    // 보상 아이템
    [SerializeField] public PortionRecipeData clearRecipe;



    [Multiline]
    [SerializeField] public string quest;

    public void CheckClear()
    {
        if (value==clearValue)
        {
            isclear = true;
            Debug.Log("트루");    
        }
        else isclear= false;
    }
}
