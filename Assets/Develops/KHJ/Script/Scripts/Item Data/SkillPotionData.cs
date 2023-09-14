using UnityEngine;

[CreateAssetMenu(fileName = "Item_Potion_Skill", menuName = "Inventory System/Item Data/Skill_Potion", order = 6)]
public class SkillPotionData : ScriptableObject
{
    [SerializeField] GameObject potionPrefab;
    [SerializeField] SkillData skillData;
}