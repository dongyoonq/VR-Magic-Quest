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
        Debug.Log($"{quest.questtitle} 퀘스트 추가");
        questList.Add(quest);
        OnQuestAdded?.Invoke(quest);
    }
    public void RemoveQuest(QuestData quest)
    {
        Debug.Log($"{quest.questtitle} 퀘스트 삭제");
        questList.Remove(quest);
        OnQuestRemoved?.Invoke(quest);
    }

    public void ClearQuest(QuestData quest)
    {
        Debug.Log($"{quest.questtitle} 퀘스트 클리어");
        questList.Remove(quest);
        OnQuestCleared?.Invoke(quest);
        OnQuestRemoved?.Invoke(quest);
        // 보상
    }

    public void KillMonster(string monsterName)
    {
       
    }

    public void GatherItem(string itemName)
    {
        
    }
}
