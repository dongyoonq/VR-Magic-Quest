using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class LightningCharge : Skill
{
    public override void CastingSpell(Player player, float value, Transform createTrans)
    {
        player.isSkillUsed = true;

        createTrans = Camera.main.transform;
        Vector3 createPos = createTrans.position + (createTrans.forward * 2f);

        Quaternion lookrot = Quaternion.LookRotation(createTrans.forward);
        Quaternion createrot = Quaternion.Euler(new Vector3(lookrot.eulerAngles.x + 90, lookrot.eulerAngles.y, lookrot.eulerAngles.z));

        Skill skill = GameManager.Resource.Instantiate(skillData.skillPrefab, createPos, createrot, true);

        LightningChargeRayCast[] l_ray = skill.GetComponentsInChildren<LightningChargeRayCast>();

        foreach (var ray in l_ray)
        {
            ray.source = skill;
            ray.player = player;
            ray.isHitted = false;
        }

        skill.StartCoroutine(MoveRotateRoutine(player, skill));
    }

    IEnumerator MoveRotateRoutine(Player player, Skill skill)
    {
        Vector3 start = skill.transform.position;
        Vector3 end = skill.transform.position + skill.transform.up * 10f;

        Vector3 startRotate = skill.transform.rotation.eulerAngles;
        Vector3 endRotate = new Vector3(startRotate.x, startRotate.y, startRotate.z + 360f);

        float time = 0f;
        float duration = 4f;

        while (time < duration)
        {
            time += Time.deltaTime;

            float t = time / duration;

            skill.transform.position = Vector3.Lerp(start, end, t);
            Vector3 currentRotation = Vector3.Lerp(startRotate, endRotate, t);

            skill.transform.rotation = Quaternion.Euler(currentRotation);
            yield return null;
        }

        player.isSkillUsed = false;

        if (skill.IsValid())
            GameManager.Resource.Destroy(skill);
    }
}