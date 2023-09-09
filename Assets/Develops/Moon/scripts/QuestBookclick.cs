using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestBookclick : MonoBehaviour
{
    public Button button;


    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Questlistclick);
    }

    public void Questlistclick()
    {
        Debug.Log("Å¬¸¯ÇÔ");
    }


}
