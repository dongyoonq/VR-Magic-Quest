using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EnumType;

public class SkillSorting : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] SkillType[] skillType;

    private SkillUI skillUI;

    private void Start()
    {
        skillUI = GetComponentInParent<SkillUI>();

        button.onClick.AddListener(() => skillUI.OnSortingSkillSlot?.Invoke(skillType));
    }
}
