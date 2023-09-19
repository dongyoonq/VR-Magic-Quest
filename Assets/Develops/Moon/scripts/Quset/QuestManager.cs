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

    public UnityAction<QuestData> OnQuestUpdated;

    public void Awake()
    {
        questList = new List<QuestData>();
    }

    public void AddQuest(QuestData quest)   
    {
        
        questList.Add(quest);
        OnQuestAdded?.Invoke(quest);
    }
    public void RemoveQuest(QuestData quest)
    {
        
        questList.Remove(quest);
        OnQuestRemoved?.Invoke(quest);
    }

    public void ClearQuest(QuestData quest)
    {
      //  Debug.Log($"{quest.questtitle} ����Ʈ Ŭ����");
        questList.Remove(quest);
        OnQuestCleared?.Invoke(quest);
        OnQuestRemoved?.Invoke(quest);
        // ����
    }

    public void UpdateQuest(QuestData quest)
    {
       
        OnQuestUpdated?.Invoke(quest);
    }


    public void KillMonster(string monsterName)
    {
   
        foreach (QuestData questData in questList)
        {
           
            if (questData.monster!= monsterName)
            {
                Debug.Log("no");
            }
            else
            {
               
                questData.value++;
                questData.CheckClear();
             
                UpdateQuest(questData);
                break;
            }
               

        }
    }

    public void GatherItem(ItemData item)
    {
        foreach (QuestData questData in questList)
        {
            
            if (questData.item != item.Name)
            {
                Debug.Log("�̻��ѵ���");

            }
            else
            {
                questData.value++;
                questData.CheckClear();
                UpdateQuest(questData);
                return;
            }


        }
    }
}
