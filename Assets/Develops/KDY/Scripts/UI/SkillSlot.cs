using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    [SerializeField] public Image skillImg;
    [SerializeField] public SkillData skillData;

    private void OnEnable()
    {
        skillImg.sprite = skillData.skillSprite;
    }
}
