using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class Questpaper : MonoBehaviour
{
    [SerializeField] QuestData quest;
    [SerializeField] Button button;
    //[SerializeField] Text text;

    public void Awake()
    {
        button.onClick.AddListener(Accpt);
    //    text =quest.quest;
    }

    public void Accpt()
    {
        Debug.Log("µé¾î°¨");
        GameObject.Find("QuestManager").gameObject.GetComponent<playerQuestList>().AddList(quest);
        Destroy(gameObject);
    }
}
