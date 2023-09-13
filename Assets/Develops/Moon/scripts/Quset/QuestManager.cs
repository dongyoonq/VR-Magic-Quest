using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public List<QuestData> questList;

    public UnityEvent<QuestData> OnQuestAdded;
    public UnityEvent<QuestData> OnQuestRemoved;
    public UnityEvent<QuestData> OnQuestCleared;

    public UnityEvent OnQuestUpdated;

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
            if (questData.monster == monsterName)
            {
                questData.value++;
                questData.CheckClear();
                Debug.Log("�ø�");
                UpdateQuest();

            }
            else
            {
                Debug.Log(questData.monster);
                Debug.Log(monsterName);
                Debug.Log("�ȿø�");
                return;
            }
               

        }
    }

    public void GatherItem(string itemName)
    {
        
    }
}
