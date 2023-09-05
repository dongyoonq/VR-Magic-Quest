using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBasic : Skill
{
    public override void CastingSpell(Player player, float value, Transform createTrans)
    {
        player.isSkillUsed = true;

        createTrans = Camera.main.transform;
        Vector3 createPos = createTrans.position + (createTrans.forward * 2f);
        Skill skill = GameManager.Resource.Instantiate(skillData.skillPrefab, createPos, Quaternion.LookRotation(createTrans.forward), true);

        SkillProjectileCollider collider = skill.GetComponentInChildren<SkillProjectileCollider>();
        collider.skillSource = skill;
        collider.source = player;

        skill.StartCoroutine(ProjectileMoveRoutine(player, skill));
    }

    IEnumerator ProjectileMoveRoutine(Player player, Skill skill)
    {
        Vector3 start = skill.transform.position;
        Vector3 end = skill.transform.position + skill.transform.forward * 5f;

        float time = 0f;
        float duration = 5f;

        while (time < 1f)
        {
            skill.transform.position = Vector3.Lerp(start, end, time);
            time += Time.deltaTime / duration;
            yield return null;
        }

        player.isSkillUsed = false;

        if (skill.IsValid())
            GameManager.Resource.Destroy(skill);
    }
}