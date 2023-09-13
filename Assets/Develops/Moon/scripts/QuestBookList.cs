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

    public void Start()
    {
        questButtonList = new List<QuestBookclick>();
        clearbutton.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
            GameManager.Quest.OnQuestAdded?.AddListener(AddQuest);
            GameManager.Quest.OnQuestRemoved?.AddListener(RemoveQuest);
            GameManager.Quest.OnQuestUpdated?.AddListener(UpDateQuest);
             InitQuest();
    }

    private void OnDisable()
    {

        GameManager.Quest.OnQuestAdded?.RemoveListener(AddQuest);
        GameManager.Quest.OnQuestRemoved?.RemoveListener(RemoveQuest);
        GameManager.Quest.OnQuestUpdated?.RemoveListener(UpDateQuest);
        ReleaseQuest();
    }


    private void InitQuest()
    {
        Debug.Log("Äù½ºÆ®ºÏ ÃÊ±âÈ­");
     //   List<QuestData> list = GameObject.Find("QuestManager").gameObject.GetComponent<QuestManager>().questList;
        List<QuestData> list = GameManager.Quest.questList;
        foreach (QuestData data in list)
        {
            AddQuest(data);
        }
    }

    private void ReleaseQuest()
    {
        Debug.Log("Äù½ºÆ®ºÏ ¸¶¹«¸®");
       // List<QuestData> list = GameObject.Find("QuestManager").gameObject.GetComponent<QuestManager>().questList;
        List<QuestData> list = GameManager.Quest.questList;
        foreach (QuestData data in list)
        {
            RemoveQuest(data);
        }
    }

    private void AddQuest(QuestData quest)
    {
        Debug.Log($"Äù½ºÆ®ºÏ Äù½ºÆ® Ãß°¡ {quest.questtitle}");
        QuestBookclick questButton = Instantiate(questButtonPrefab, questButtonRoot);
        questButton.quest = quest;
        questButton.bookList = this;
        questButton.gameObject.name = quest.questtitle;
        questButton.qusettitle.text = quest.questtitle;
        questButtonList.Add(questButton);
       
    }

    public void UpDateQuest()
    {
        //  List<QuestData> list = GameObject.Find("QuestManager").gameObject.GetComponent<QuestManager>().questList;
        List<QuestData> list = GameManager.Quest.questList;
        foreach (QuestData data in list)
        {
            if (data.isclear)
            {
                QuestData quest = data;
                clearbutton.gameObject.SetActive(true);
                clearbutton.onClick.AddListener(() => Clear(quest));
            }
            else
            {
                clearbutton.gameObject.SetActive(false);
            }
        }
    }

    public void RemoveQuest(QuestData quest)
    {
        Debug.Log($"Äù½ºÆ®ºÏ Äù½ºÆ® Á¦°Å {quest.questtitle}");
        QuestBookclick questButton = questButtonList.Find(x => x.quest == quest);
        if(questButton != null)
        {
            questButtonList.Remove(questButton);
            Destroy(questButton.gameObject);
        }
        Debug.Log("»èÁ¦");
    }

    public void SetCurQuest(QuestData quest)
    {
        Debug.Log($"ÇöÀç Äù½ºÆ® ¼³Á¤ {quest.questtitle}");
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
        Debug.Log("º¸»ó");
        QuestBookclick questButton = questButtonList.Find(x => x.quest == quest);
        if (questButton != null)
        {
            Destroy(questButton.gameObject);
        }
        clearbutton.gameObject.SetActive(false);
        questDetail.text = "";
        GameManager.Quest.ClearQuest(curQuestData);
    }
}
