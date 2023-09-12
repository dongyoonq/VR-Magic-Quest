using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "QuestData", menuName = "Scriptable Object/QuestData")]
public class QuestData : ScriptableObject
{
    [SerializeField] public string questtitle;
    [SerializeField] public string quest;
    [SerializeField] public int value;
    [SerializeField] public bool isclear;
    [SerializeField] public bool isinventory;
    [SerializeField] public string monster;
    // 보상 아이템
    [SerializeField] public GameObject Object;

    public void CheckClear()
    {
        if (value == 1)
        {
            isclear = true;
            Debug.Log("트루");
        }
        else isclear= false;
    }
}
