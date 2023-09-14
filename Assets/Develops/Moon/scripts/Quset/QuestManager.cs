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
            if (questData.monster!= monsterName)
            {
                Debug.Log("이상한데들어감");
            }
            else
            {
               
                questData.value++;
                questData.CheckClear();
                Debug.Log("올림");
                UpdateQuest();
                break;
            }
               

        }
    }

    public void GatherItem(string itemName)
    {
        foreach (QuestData questData in questList)
        {
            Debug.Log("들어감");
            if (questData.item != itemName)
            {
                Debug.Log("이상한데들어감");

            }
            else
            {
                ++questData.value;
                questData.CheckClear();
                Debug.Log("올림");
                UpdateQuest();
                return;
            }


        }
    }
}
