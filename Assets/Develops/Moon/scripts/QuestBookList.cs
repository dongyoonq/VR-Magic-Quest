using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class QuestBookList : MonoBehaviour
{
    [SerializeField] public QuestData curQuestData;

    [SerializeField] public Transform questButtonRoot;
    [SerializeField] public QuestBookclick questButtonPrefab;
    [SerializeField] public List<QuestBookclick> questButtonList;
    [SerializeField] public Button clearbutton;

    [SerializeField] private TMP_Text questDetail;

    public Player player;
    public void Awake()
    {
        questButtonList = new List<QuestBookclick>();
        clearbutton.gameObject.SetActive(false);
    }
    

    private void OnEnable()
    {
        GameManager.Quest.OnQuestAdded += AddQuest;
        GameManager.Quest.OnQuestRemoved += RemoveQuest;
         GameManager.Quest.OnQuestUpdated+=UpDateQuest;
        InitQuest();
    }

    private void OnDisable()
    {
        GameManager.Quest.OnQuestAdded -=AddQuest;
        GameManager.Quest.OnQuestRemoved -=RemoveQuest;
        GameManager.Quest.OnQuestUpdated-=UpDateQuest;
        ReleaseQuest();

    }


    private void InitQuest()
    {
       
        List<QuestData> list = GameManager.Quest.questList;
        foreach (QuestData data in list)
        {
            AddQuest(data);
        }
    }

    private void ReleaseQuest()
    {
       
        List<QuestData> list = GameManager.Quest.questList;
        foreach (QuestData data in list)
        {
            RemoveQuest(data);
        }
    }

    private void AddQuest(QuestData quest)
    {
        
        QuestBookclick questButton = Instantiate(questButtonPrefab, questButtonRoot);
        questButton.quest = quest;
        questButton.bookList = this;
        questButton.gameObject.name = quest.questtitle;
        questButton.qusettitle.text = quest.questtitle;
        questButtonList.Add(questButton);
    }

    public void UpDateQuest(QuestData quest)
    {
     
        List<QuestData> list = GameManager.Quest.questList;
        if(curQuestData == null)
        {
            return;
        }
        if (curQuestData.isclear)
        {
            clearbutton.gameObject.SetActive(true);
            clearbutton.onClick.AddListener(() => Clear(quest));
        }
        else
        {
            clearbutton.gameObject.SetActive(false);
        }


    }

    public void RemoveQuest(QuestData quest)
    {
       
        QuestBookclick questButton = questButtonList.Find(x => x.quest == quest);
        if(questButton != null)
        {
            questButtonList.Remove(questButton);
            Destroy(questButton.gameObject);
        }
    
    }

    public void SetCurQuest(QuestData quest)
    {
     
        curQuestData = quest;
        questDetail.text = quest.quest;

        if (quest.isclear)
        {
            clearbutton.gameObject.SetActive(true);
            clearbutton.onClick.AddListener(()=>Clear(quest));
        }
        else
        {
            clearbutton.gameObject.SetActive(false);
        }
         
    }


    public void Clear(QuestData quest)
       {
       
        QuestBookclick questButton = questButtonList.Find(x => x.quest == quest);
        if (questButton != null)
        {
            Destroy(questButton.gameObject);
            player.UnlockRecipe(quest.clearRecipe);
            GameManager.Quest.ClearQuest(quest);
            curQuestData = null;
            clearbutton.gameObject.SetActive(false);
            questDetail.text = "";
        }
        curQuestData = null;
    }
}
