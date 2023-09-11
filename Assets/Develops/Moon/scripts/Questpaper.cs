using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class Questpaper : MonoBehaviour
{
    [SerializeField] QuestBookList questlist;

    [SerializeField] QuestData questdata;
    [SerializeField] Button button;

    [SerializeField] QuestBookclick instatebutton;
    [SerializeField] GameObject questlistroot;
    //[SerializeField] Text text;

    public void Awake()
    {
        //    questlist.OnQuested += Accpt;
        //     button.onClick.AddListener(()=> questlist.OnQuested());
          button.onClick.AddListener(Accpt);
    }

    public void Accpt()
    {
        Debug.Log("µé¾î°¨");
        GameObject.Find("QuestManager").gameObject.GetComponent<playerQuestList>().AddList(questdata);
        instatebutton.quest = questdata;
        GameObject obj = Instantiate(instatebutton.gameObject, questlistroot.transform);
        Destroy(gameObject);
    }
}
