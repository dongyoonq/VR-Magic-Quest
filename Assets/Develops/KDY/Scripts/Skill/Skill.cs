using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [SerializeField] public SkillData skillData;

    public abstract void CastingSpell(Player player, float value, Transform createTrans = null);
}