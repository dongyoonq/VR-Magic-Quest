using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_Portion_", menuName = "Inventory System/Item Data/SkillPortion", order = 6)]
public class SkillPortionItemData : PortionItemData
{
    public SkillData SkillData => skillData;
    [SerializeField] private SkillData skillData;
}
