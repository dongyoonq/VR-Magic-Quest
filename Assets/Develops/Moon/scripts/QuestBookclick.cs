using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestBookclick : MonoBehaviour
{
    public QuestBookList bookList;
    public QuestData quest;

    public TMP_Text qusettitle;

    public void Questlistclick()
    {
        bookList.SetCurQuest(quest);
    }
}
