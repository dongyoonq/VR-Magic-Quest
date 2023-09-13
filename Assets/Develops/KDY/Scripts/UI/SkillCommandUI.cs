using PDollarGestureRecognizer;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillCommandUI : MonoBehaviour
{
    [SerializeField] public Image skillImage;
    [SerializeField] public TMP_Text skillText;
    [SerializeField] public TMP_Text infoText;
    [SerializeField] public TMP_Text recognzieText;
    [SerializeField] public RectTransform contents;
    [SerializeField] List<SkillCommandButton> commandButtons;
    [SerializeField] SkillCommandButton commandButtonPrefab;
    [SerializeField] SkillUI skillUI;

    public UnityEvent<string> OnSetCommanded;
    public SkillData selectSkill;

    private void Start()
    {
        OnSetCommanded.AddListener(SetCommandedUIUpdate);
    }

    private void OnEnable()
    {
        foreach (SkillCommandButton button in commandButtons)
        {
            Destroy(button.gameObject);
        }

        commandButtons.Clear();

        foreach (Gesture gesture in skillUI.player.trainingSet)
        {
            SkillCommandButton button = GameManager.Resource.Instantiate(commandButtonPrefab, true);
            button.transform.SetParent(contents, false);
            button.OnUpdateCommand?.Invoke(gesture.Name);
            commandButtons.Add(button);
        }
    }

    public void CommandButtonUpdate()
    {
        foreach (SkillCommandButton button in commandButtons)
        {
            Destroy(button.gameObject);
        }

        foreach (SkillData skillData in skillUI.player.skillList)
        {
            SkillCommandButton button = GameManager.Resource.Instantiate(commandButtonPrefab, contents, true);
            button.OnUpdateCommand?.Invoke(skillData.recognizeGestureName);
            commandButtons.Add(button);
        }
    }

    private void SetCommandedUIUpdate(string gestureName)
    {
        if (!skillUI.player.skillList.Contains(selectSkill))
        {
            infoText.color = Color.red;
            infoText.text = "You must learn this skill first";
            recognzieText.text = "Current Recognize : ";
        }
        else
        {
            infoText.color = Color.blue;
            infoText.text = "Command registration is success";
            recognzieText.text = $"Current Recognize : {gestureName}";
            selectSkill.recognizeGestureName = gestureName;
            skillUI.OnSkillCommandUpdate?.Invoke(gestureName);
        }
    }
}
