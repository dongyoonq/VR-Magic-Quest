using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] TMP_Text questpanel;

    public void Start()
    {
        questpanel.text = questdata.quest;
    }
    public void Accept()
    {
    
        Debug.Log("����Ʈ �ޱ�");
        QuestData quest = ScriptableObject.CreateInstance<QuestData>();
        quest.name = questdata.name;
        quest.questtitle = questdata.questtitle;
        quest.quest = questdata.quest;
        quest.value = questdata.value;
        quest.isclear = questdata.isclear;
        quest.monster = questdata.monster;
        quest.item = questdata.item;
        quest.clearValue = questdata.clearValue;
        quest.Object = questdata.Object;

        // TODO : QuestManager �̱������� ���� Find ���� �ʱ�
        //  GameObject.Find("QuestManager").gameObject.GetComponent<QuestManager>().AddQuest(quest);
        GameManager.Quest.AddQuest(quest);
        Destroy(gameObject);
    }
}
