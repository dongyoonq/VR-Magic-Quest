using System.ComponentModel.Design;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static EnumType;

public class SkillUI : MonoBehaviour
{
    [SerializeField] SkillInfoUI infoUI;
    [SerializeField] SkillCommandUI commandUI;
    [SerializeField] SkillSlot[] skillSlots;
    [SerializeField] MP4Loader mp4Loader;
    
    private Player player;

    public UnityEvent<SkillData> OnPointerSkllSlot;
    public UnityEvent<string> OnSkillCommandUpdate;
    public UnityEvent<SkillType[]> OnSortingSkillSlot;
    public UnityEvent OnPlayerSkillUIUpdate;

    public bool isSelectedBtn = false;

    private void Start()
    {
        player = FindAnyObjectByType<Player>();

        OnPointerSkllSlot.AddListener(ShowSkillDescription);
        OnSortingSkillSlot.AddListener(SortingSkill);
        OnSkillCommandUpdate.AddListener(UpdateGestureInfoCommand);
        OnPlayerSkillUIUpdate.AddListener(PlayerActiveSkillCheck);

        skillSlots = GetComponentsInChildren<SkillSlot>();
        OnPlayerSkillUIUpdate?.Invoke();
    }

    private void ShowSkillDescription(SkillData skillData)
    {
        mp4Loader.OffLoad();

        commandUI.selectSkill = skillData;
        commandUI.skillText.text = skillData.skillName;
        commandUI.recognzieText.text = $"Current Recognize : {skillData.recognizeGestureName}";
        commandUI.skillImage.sprite = skillData.skillSprite;

        infoUI.nameText.text = skillData.skillName;
        infoUI.infoText.text = $"Damage : {skillData.damage}\nUseMp : {skillData.useMp}";
        infoUI.gestureText.text = $"Skill Recognize : {skillData.recognizeGestureName}";
        infoUI.descriptionText.text = skillData.skillTooltip;
        infoUI.skillImage.sprite = skillData.skillSprite;

        infoUI.recognizeBtn.onClick.RemoveAllListeners();

        if (!string.IsNullOrEmpty(skillData.recognizeGestureName))
            infoUI.recognizeBtn.onClick.AddListener(() => mp4Loader.OnLoad(FindVideoFileInfo(skillData.recognizeGestureName)));

        if (player.skillList.Contains(skillData))
        {
            infoUI.subscribeText.color = Color.blue;
            infoUI.subscribeText.text = "You can use this skill";   

            commandUI.infoText.color = Color.blue;
            commandUI.infoText.text = "You can register this skill";
        }
        else
        {
            infoUI.subscribeText.color = Color.red;
            infoUI.subscribeText.text = "You must learn this skill first";

            commandUI.infoText.color = Color.red;
            commandUI.infoText.text = "You can't register this skill, must learn this skill first";
        }
    }

    private void UpdateGestureInfoCommand(string gestureName)
    {
        infoUI.recognizeBtn.onClick.RemoveAllListeners();
        infoUI.recognizeBtn.onClick.AddListener(() => mp4Loader.OnLoad(FindVideoFileInfo(gestureName)));

        infoUI.gestureText.text = $"Skill Recognize : {gestureName}";
    }
    
    private void PlayerActiveSkillCheck()
    {
        foreach (SkillSlot slot in skillSlots)
        {
            Image image = slot.GetComponent<Image>();
            RectTransform rect = slot.GetComponent<RectTransform>();

            if (player.skillList.Contains(slot.skillData))
            {
                // 활성화 색상
                image.color = new Color(1f, 0.95f, 0f);
                rect.SetAsFirstSibling();
            }
            else
            {
                // 비활성화 색상
                image.color = new Color(0.55f, 0.54f, 0.5f);
                rect.SetAsLastSibling();
            }
        }
    }

    private void SortingSkill(SkillType[] skillTypes)
    {
        // All
        if (skillTypes.Length > 1)
        {
            foreach (SkillSlot slot in skillSlots)
            {
                slot.gameObject.SetActive(true);
            }

            return;
        }

        SkillType skillType = skillTypes[0];

        // Sorting
        foreach (SkillSlot slot in skillSlots)
        {
            if (slot.skillData.skillType == skillType)
            {
                slot.gameObject.SetActive(true);
            }
            else
            {
                slot.gameObject.SetActive(false);
            }
        }
    }

    private FileInfo FindVideoFileInfo(string gestureName)
    {
        DirectoryInfo di = new DirectoryInfo(Application.dataPath + "\\Editors\\Record\\");

        foreach (FileInfo file in di.GetFiles())
        {
            if (file.Name == gestureName + " Video.mp4")
            {
                return file;
            }
        }

        return null;
    }
}