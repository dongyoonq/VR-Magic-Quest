using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillCommandButton : MonoBehaviour
{
    public UnityEvent<string> OnUpdateCommand;

    SkillCommandUI commandUI;
    Button button;
    TMP_Text buttonName;

    private string gestureName;

    private void Start()
    {
        commandUI = GetComponentInParent<SkillCommandUI>();
    }

    private void OnEnable()
    {
        button = GetComponent<Button>();
        buttonName = GetComponentInChildren<TMP_Text>();

        OnUpdateCommand.AddListener(UpdateCommand);
        button.onClick.AddListener(() => SetCommand());
    }

    private void OnDisable()
    {
        OnUpdateCommand.RemoveAllListeners();
        button.onClick.RemoveAllListeners();
    }

    private void SetCommand()
    {
        commandUI.OnSetCommanded?.Invoke(gestureName);
    }

    private void UpdateCommand(string gestureName)
    {
        this.gestureName = gestureName;
        buttonName.text = this.gestureName;
    }
}
