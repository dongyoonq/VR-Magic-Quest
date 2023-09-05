using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBasic : Skill
{
    public override void CastingSpell(Player player, float value, Transform createTrans)
    {
        createTrans = Camera.main.transform;
        Vector3 createPos = createTrans.position + (createTrans.forward * 2f);
        Skill skill = GameManager.Resource.Instantiate(skillData.skillPrefab, createPos, Quaternion.LookRotation(createTrans.forward), true);
        skill.GetComponentInChildren<FireBasicCollider>().skillData = skill.skillData;
        skill.StartCoroutine(ProjectileMoveRoutine(skill));
    }

    IEnumerator ProjectileMoveRoutine(Skill skill)
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

        if (skill.IsValid())
            GameManager.Resource.Destroy(skill);
    }
}