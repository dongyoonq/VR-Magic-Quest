using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenomBeam : Skill
{
    public override void CastingSpell(Player player, float value, Transform createTrans)
    {
        player.isSkillUsed = true;

        createTrans = Camera.main.transform;
        Vector3 createPos = createTrans.position + (createTrans.forward * 2f);
        Skill skill = GameManager.Resource.Instantiate(skillData.skillPrefab, createPos, Quaternion.LookRotation(createTrans.forward), true);

        SkillProjectileRaycast cast = skill.GetComponentInChildren<SkillProjectileRaycast>();
        cast.source = skill;
        cast.player = player;
        cast.hitTime = 0.2f;
    }
}