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
        Debug.Log("����Ʈ �ޱ�");
        QuestData quest = ScriptableObject.CreateInstance<QuestData>();
        quest.name = questdata.name;
        quest.questtitle = questdata.questtitle;
        quest.quest = questdata.quest;
        quest.isinventory = questdata.isinventory;
        quest.isclear = questdata.isclear;

        // TODO : QuestManager �̱������� ���� Find ���� �ʱ�
        GameObject.Find("QuestManager").gameObject.GetComponent<QuestManager>().AddQuest(quest);
        Destroy(gameObject);
    }
}
