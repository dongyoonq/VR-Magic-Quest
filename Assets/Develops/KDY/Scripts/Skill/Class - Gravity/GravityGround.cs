using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGround : Skill
{
    private bool isDamaged;

    public override void CastingSpell(Player player, float value, Transform createTrans)
    {
        isDamaged = false;
        player.isSkillUsed = true;

        createTrans = Camera.main.transform;
        Vector3 createPos = createTrans.position + (createTrans.forward * 6f);
        Skill skill = GameManager.Resource.Instantiate(skillData.skillPrefab, createPos, Quaternion.identity, true);

        skill.StartCoroutine(AttackJudgement(player, skill));
    }

    IEnumerator AttackJudgement(Player player, Skill skill)
    {
        yield return new WaitForSeconds(0.3f);

        float time = 0f;

        while (time < 5f)
        {
            time += Time.deltaTime;
            Collider[] colliders = Physics.OverlapBox(skill.transform.position, new Vector3(5f, 1f, 5f) / 2, Quaternion.identity, LayerMask.GetMask("Monster"));
            
            foreach (Collider collider in colliders)
            {
                IHittable hitMonster = collider.GetComponent<IHittable>();

                // Binding

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
        yield return new WaitForSeconds(1f);
        isDamaged = false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(5f, 1f, 5f));
    }
}