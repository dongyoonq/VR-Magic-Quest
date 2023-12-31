using System.Collections;
using UnityEngine;

public class FireBeam : Skill
{
    public override void CastingSpell(Player player, float correctValue, Transform createTrans)
    {
        player.isSkillUsed = true;

        Vector3 direction = createTrans.up;
        Skill skill = GameManager.Resource.Instantiate(skillData.skillPrefab, createTrans.position, Quaternion.LookRotation(direction), createTrans, true);
        skill.StartCoroutine(AttackJudgement(player, skill, createTrans));
    }

    IEnumerator AttackJudgement(Player player, Skill skill, Transform createTrans)
    {
        float time = 0f;
        float hitIntervalTime = 1f;

        while (time < 5f)
        {
            time += Time.deltaTime;
            hitIntervalTime += Time.deltaTime;

            if (Physics.Raycast(createTrans.position, createTrans.up, out RaycastHit hitInfo, 15f, LayerMask.GetMask("Monster")) && hitIntervalTime >= 1f)
            {
                IHittable hitMonster = hitInfo.collider.GetComponent<IHittable>();
                IHitReactor hitReactor = hitInfo.collider.GetComponent<IHitReactor>();
                hitMonster.TakeDamaged(skill.skillData.damage);
                hitReactor.HitReact(skill.skillData.hitTags, 0.1f);
                hitIntervalTime = 0f;
            }

            yield return null;
        }

        player.isSkillUsed = false;
        GameManager.Resource.Destroy(skill);
    }
}