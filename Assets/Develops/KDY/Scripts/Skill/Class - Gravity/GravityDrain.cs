using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityDrain : Skill
{
    public override void CastingSpell(Player player, float value, Transform createTrans)
    {
        player.isSkillUsed = true;

        createTrans = Camera.main.transform;
        Vector3 createPos = createTrans.position + (createTrans.forward * 2f);
        Skill skill = GameManager.Resource.Instantiate(skillData.skillPrefab, createPos, Quaternion.LookRotation(createTrans.forward), true);

        GravityDrainCollider g_Collider = skill.GetComponentInChildren<GravityDrainCollider>();
        g_Collider.skillSource = skill;
        g_Collider.source = player;

        skill.StartCoroutine(SkillRoutine(player, skill));
    }

    IEnumerator SkillRoutine(Player player, Skill skill)
    {
        yield return new WaitForSeconds(5f);
        player.isSkillUsed = false;

        if (skill.IsValid())
            GameManager.Resource.Destroy(skill);
    }
}