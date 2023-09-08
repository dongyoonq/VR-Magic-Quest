using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class QuestBookList : MonoBehaviour
{
    [SerializeField] Quest quest;
    [SerializeField] GameObject instatebutton;
    [SerializeField] TextMeshPro text;
    [SerializeField] GameObject questlistroot;

   public void Update()
    {
        foreach(var quest in quest.Questlist)
        {
            if (quest != null)
            {
                Debug.Log(quest.questtitle);
               GameObject obj = Instantiate(instatebutton, questlistroot.transform.GetChild(0).transform);
              
            }
        }
    }
    



}
