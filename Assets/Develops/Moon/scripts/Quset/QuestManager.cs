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

    private void Awake()
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

    public void KillMonster(string monsterName)
    {
       
    }

    public void GatherItem(string itemName)
    {
        
    }
}
