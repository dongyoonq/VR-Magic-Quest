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
    [SerializeField] int value;
    [SerializeField] public bool isclear;
    [SerializeField] public bool isinventory;

    // 보상 아이템
    [SerializeField] public GameObject Object;

    public void CheckClear()
    {
        isclear = true;
        /*  if (isclear)
              return;

          if (true)
          {
              isclear = true;
          }*/
    }
}
