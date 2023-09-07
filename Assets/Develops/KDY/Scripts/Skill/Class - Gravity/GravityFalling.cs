using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityFalling : Skill
{
    public override void CastingSpell(Player player, float value, Transform createTrans)
    {
        player.isSkillUsed = true;

        createTrans = Camera.main.transform;
        Vector3 createPos = createTrans.position + (createTrans.forward * 8f) + (createTrans.up * -2f);
        Skill skill = GameManager.Resource.Instantiate(skillData.skillPrefab, createPos, Quaternion.LookRotation(createTrans.forward), false);

        GravityFallingCollider g_Collider = skill.GetComponentInChildren<GravityFallingCollider>();
        g_Collider.skillSource = skill;
        g_Collider.source = player;

        skill.StartCoroutine(FallingRoutine(player, skill));
    }

    IEnumerator FallingRoutine(Player player, Skill skill)
    {
        yield return new WaitForSeconds(4f);

        player.isSkillUsed = false;

        if (skill.IsValid())
            GameManager.Resource.Destroy(skill.gameObject);
    }
}