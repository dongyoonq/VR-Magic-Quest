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
    [SerializeField] QuestData questdata;

    public void Accept()
    {
        Debug.Log("퀘스트 받기");
        QuestData quest = ScriptableObject.CreateInstance<QuestData>();
        quest.name = questdata.name;
        quest.questtitle = questdata.questtitle;
        quest.quest = questdata.quest;
        quest.isinventory = questdata.isinventory;
        quest.isclear = questdata.isclear;

        // TODO : QuestManager 싱글톤으로 만들어서 Find 쓰지 않기
        GameObject.Find("QuestManager").gameObject.GetComponent<QuestManager>().AddQuest(quest);
        Destroy(gameObject);
    }
}
