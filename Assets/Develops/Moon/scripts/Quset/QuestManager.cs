using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public List<QuestData> questList;

    public UnityAction<QuestData> OnQuestAdded;
    public UnityAction<QuestData> OnQuestRemoved;
    public UnityAction<QuestData> OnQuestCleared;

    public UnityAction OnQuestUpdated;

    public void Awake()
    {
        questList = new List<QuestData>();
    }

    public void AddQuest(QuestData quest)   
    {
        Debug.Log($"{quest.questtitle} ����Ʈ �߰�");
        questList.Add(quest);
        OnQuestAdded?.Invoke(quest);
    }
    public void RemoveQuest(QuestData quest)
    {
        Debug.Log($"{quest.questtitle} ����Ʈ ����");
        questList.Remove(quest);
        OnQuestRemoved?.Invoke(quest);
    }

    public void ClearQuest(QuestData quest)
    {
        Debug.Log($"{quest.questtitle} ����Ʈ Ŭ����");
        questList.Remove(quest);
        OnQuestCleared?.Invoke(quest);
        OnQuestRemoved?.Invoke(quest);
        // ����
    }

    public void UpdateQuest()
    {
        Debug.Log("����Ʈ ������Ʈ");
        OnQuestUpdated?.Invoke();
    }


    public void KillMonster(string monsterName)
    {
   
        foreach (QuestData questData in questList)
        {
            Debug.Log("��");
            if (questData.monster!= monsterName)
            {
                Debug.Log("�̻��ѵ���");
            }
            else
            {
               
                questData.value++;
                questData.CheckClear();
                Debug.Log("�ø�");
                UpdateQuest();
                break;
            }
               

        }
    }

    public void GatherItem(string itemName)
    {
        foreach (QuestData questData in questList)
        {
            Debug.Log("��");
            if (questData.item != itemName)
            {
                Debug.Log("�̻��ѵ���");

            }
            else
            {
                ++questData.value;
                questData.CheckClear();
                Debug.Log("�ø�");
                UpdateQuest();
                return;
            }


        }
    }
}
