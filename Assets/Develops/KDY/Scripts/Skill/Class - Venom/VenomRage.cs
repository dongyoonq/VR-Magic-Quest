using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenomRage : Skill
{
    public override void CastingSpell(Player player, float value, Transform createTrans)
    {
        player.isSkillUsed = true;

        createTrans = Camera.main.transform;
        Vector3 createPos = createTrans.position;
        Skill skill = GameManager.Resource.Instantiate(skillData.skillPrefab, createPos, Quaternion.LookRotation(createTrans.forward), true);

        skill.StartCoroutine(RageAttackRoutine(skill));
        skill.StartCoroutine(RageEndRoutine(player, skill));
    }

    private void PlayerBuff(Player player)
    {
        // Todo : Player Buff
    }

    IEnumerator RageAttackRoutine(Skill skill)
    {
        yield return new WaitForSeconds(0.1f);

        Collider[] colliders = Physics.OverlapSphere(skill.transform.position, 6f, LayerMask.GetMask("Monster"));

        foreach (Collider collider in colliders)
        {
            IHittable hitMonster = collider.GetComponent<IHittable>();
            IHitReactor hitReactor = collider.GetComponent<IHitReactor>();

            if (hitReactor != null)
            {
                hitReactor.HitReact(skillData.hitTags, 0.2f);
            }

            if (hitMonster != null)
            {
                hitMonster.TakeDamaged(skillData.damage);
            }
        }
    }

    IEnumerator RageEndRoutine(Player player, Skill skill)
    {
        yield return new WaitForSeconds(3f);

        player.isSkillUsed = false;

        if (skill.IsValid())
            GameManager.Resource.Destroy(skill);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, 6f);
    }
}