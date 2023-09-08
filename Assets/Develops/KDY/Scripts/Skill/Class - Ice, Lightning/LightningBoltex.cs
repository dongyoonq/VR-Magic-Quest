using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBoltex : Skill
{
    public override void CastingSpell(Player player, float value, Transform createTrans)
    {
        player.isSkillUsed = true;

        createTrans = Camera.main.transform;
        Vector3 createPos = createTrans.position + (createTrans.forward * 2f);

        Skill skill = GameManager.Resource.Instantiate(skillData.skillPrefab, createPos, Quaternion.LookRotation(createTrans.forward), true);

        SkillProjectileCollider l_collider = skill.GetComponentInChildren<SkillProjectileCollider>();
        l_collider.skillSource = skill;
        l_collider.source = player;

        skill.StartCoroutine(SkillRoutine(player, skill));
    }

    IEnumerator SkillRoutine(Player player, Skill skill)
    {
        yield return new WaitForSeconds(2.5f);

        player.isSkillUsed = false;
        
        if (skill.IsValid())
            GameManager.Resource.Destroy(skill);
    }
}