using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class playerQuestList : MonoBehaviour
{
    [SerializeField] Quest qusetlist;

    public void AddList(QuestData quest)   
    {
        qusetlist.Questlist.Add(quest);
    }


}
