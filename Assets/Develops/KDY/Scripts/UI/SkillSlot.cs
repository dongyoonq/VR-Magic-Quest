using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] public Image skillImg;
    [SerializeField] public SkillData skillData;

    private SkillUI skillUI;
    private Button button;

    private void Start()
    {
        skillUI = GetComponentInParent<SkillUI>();

        button = GetComponent<Button>();
        button.onClick.AddListener(() => 
        {
            skillUI.isSelectedBtn = true;

            skillUI.OnPointerSkllSlot?.Invoke(skillData);
        });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!skillUI.isSelectedBtn)
        {
            skillUI.OnPointerSkllSlot?.Invoke(skillData);
        }
    }

    private void OnEnable()
    {
        skillImg.sprite = skillData.skillSprite;
    }
}