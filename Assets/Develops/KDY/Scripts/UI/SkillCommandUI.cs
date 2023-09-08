using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SkillCommandUI : MonoBehaviour
{
    [SerializeField] public TMP_Text skillText;
    [SerializeField] public TMP_Text infoText;
    [SerializeField] public TMP_Text recognzieText;
    [SerializeField] public RectTransform contents;
    [SerializeField] List<SkillCommandButton> commandButtons;
    [SerializeField] SkillCommandButton commandButtonPrefab;
    [SerializeField] SkillUI skillUI;

    public UnityEvent<string> OnSetCommanded;

    public Player player;

    private void Start()
    {
        player = FindAnyObjectByType<Player>();
        OnSetCommanded.AddListener(SetCommandedUIUpdate);

        foreach (SkillData skillData in player.skillList)
        {
            SkillCommandButton button = GameManager.Resource.Instantiate(commandButtonPrefab, true);
            button.transform.SetParent(contents, false);
            button.OnUpdateCommand?.Invoke(skillData.recognizeGestureName);
            commandButtons.Add(button);
        }
    }

    public SkillData selectSkill;

    public void CommandButtonUpdate()
    {
        foreach (SkillCommandButton button in commandButtons)
        {
            Destroy(button.gameObject);
        }

        foreach (SkillData skillData in player.skillList)
        {
            SkillCommandButton button = GameManager.Resource.Instantiate(commandButtonPrefab, contents, true);
            button.OnUpdateCommand?.Invoke(skillData.recognizeGestureName);
            commandButtons.Add(button);
        }
    }

    private void SetCommandedUIUpdate(string gestureName)
    {
        if (!player.skillList.Contains(selectSkill))
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
