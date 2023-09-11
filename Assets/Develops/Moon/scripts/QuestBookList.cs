using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestBookList : MonoBehaviour
{

    [SerializeField] Quest quest;
    [SerializeField] GameObject instatebutton;
    [SerializeField] TMP_Text text;
    [SerializeField] GameObject questlistroot;

    public UnityAction OnQuested;

    public void Awake()
    {   
      //  OnQuested += QuestListCheck;
    
    }

    /* public void QuestListCheck()
     {
         foreach (var quest in quest.Questlist)
         {
             if (quest != null)
             {
                 Debug.Log(quest.questtitle);
                 GameObject obj = Instantiate(instatebutton, questlistroot.transform);
             }
         }
     }*/



}
