using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceGround : Skill
{
    private bool isDamaged;

    public override void CastingSpell(Player player, float value, Transform createTrans)
    {
        isDamaged = false;
        player.isSkillUsed = true;

        createTrans = Camera.main.transform;
        Vector3 createPos;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hitInfo, 6f))
        {
            createPos = hitInfo.point + (hitInfo.normal * 0.5f);
        }
        else
        {
            createPos = createTrans.position + (createTrans.forward * 5f);
        }

        Skill skill = GameManager.Resource.Instantiate(skillData.skillPrefab, createPos, Quaternion.identity, true);

        skill.StartCoroutine(AttackJudgement(player, skill));
    }

    IEnumerator AttackJudgement(Player player, Skill skill)
    {
        yield return new WaitForSeconds(0.3f);

        float time = 0f;

        Vector3 ceneter = skill.transform.position;
        Vector3 boxSzie = new Vector3(5f, 1f, 5f);
        Collider[] colliders = Physics.OverlapBox(ceneter, boxSzie / 2f, Quaternion.identity, LayerMask.GetMask("Monster"));

        foreach (Collider collider in colliders)
        {
            IHitReactor hitReactor = collider.GetComponent<IHitReactor>();

            if (hitReactor != null)
                hitReactor.HitReact(skillData.hitTags, 2.4f);
        }

        while (time < 2.4f)
        {
            time += Time.deltaTime;
            colliders = Physics.OverlapBox(ceneter, boxSzie / 2f, Quaternion.identity, LayerMask.GetMask("Monster"));
            
            foreach (Collider collider in colliders)
            {
                IHittable hitMonster = collider.GetComponent<IHittable>();

                // Damage
                if (!isDamaged)
                {
                    hitMonster.TakeDamaged(skill.skillData.damage);
                    isDamaged = true;
                    skill.StartCoroutine(ActiveDamageTimer());
                }
            }

            yield return null;
        }

        player.isSkillUsed = false;

        if (skill.IsValid())
            GameManager.Resource.Destroy(skill);
    }

    IEnumerator ActiveDamageTimer()
    {
        yield return new WaitForSeconds(0.8f);
        isDamaged = false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(5f, 1f, 5f));
    }
}