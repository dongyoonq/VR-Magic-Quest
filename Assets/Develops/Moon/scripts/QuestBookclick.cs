using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestBookclick : MonoBehaviour
{
    public Button button;
    [SerializeField] public QuestData quest;
    [SerializeField] public TMP_Text qusettitle;
    [SerializeField] public Questdetail questdetail;
    public void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Questlistclick);
        qusettitle.text = quest.questtitle;

    }

    public void Questlistclick()
    {
        GameObject.Find("Questdetail").gameObject.GetComponent<Questdetail>().questdetailtext.text = quest.quest;
    }


}
