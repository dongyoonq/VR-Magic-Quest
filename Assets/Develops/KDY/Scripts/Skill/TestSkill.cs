using UnityEngine;

public class TestSkill : Skill
{
    public override void CastingSpell(Player player, float value, Transform createTrans)
    {
        GameManager.Resource.Instantiate(skillData.skillPrefab, Camera.main.transform.position + (Camera.main.transform.forward * 6f), Quaternion.identity, true);
    }
}