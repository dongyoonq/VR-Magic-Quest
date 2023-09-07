using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static EnumType;

public class SkillUI : MonoBehaviour
{
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] SkillSlot[] skillSlots;
    
    private Player player;

    public UnityEvent<SkillSlot> OnPointerSkllSlot;
    public UnityEvent<SkillType[]> OnSortingSkillSlot;
    public UnityEvent OnPlayerSkillUIUpdate;

    private void Start()
    {
        player = FindAnyObjectByType<Player>();

        OnPointerSkllSlot.AddListener(ShowSkillDescription);
        OnSortingSkillSlot.AddListener(SortingSkill);
        OnPlayerSkillUIUpdate.AddListener(PlayerActiveSkillCheck);

        skillSlots = GetComponentsInChildren<SkillSlot>();
        OnPlayerSkillUIUpdate?.Invoke();
    }

    private void ShowSkillDescription(SkillSlot slot)
    {
        descriptionText.text = slot.skillData.skillTooltip;
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
}