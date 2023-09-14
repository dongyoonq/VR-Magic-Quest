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

    public void UpdateQuest()
    {
        Debug.Log("퀘스트 업데이트");
        OnQuestUpdated?.Invoke();
    }


    public void KillMonster(string monsterName)
    {
   
        foreach (QuestData questData in questList)
        {
            Debug.Log("들어감");
            if (questData.monster == monsterName)
            {
                questData.value++;
                questData.CheckClear();
                Debug.Log("올림");
                UpdateQuest();

            }
            else
            {
                Debug.Log(questData.monster);
                Debug.Log(monsterName);
                Debug.Log("안올림");
                return;
            }
               

        }
    }

    public void GatherItem(string itemName)
    {
        
    }
}
