using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FireCircle : Skill
{
    private bool isDamaged;

    public override void CastingSpell(Player player, float correctValue, Transform createTrans)
    {
        isDamaged = false;
        player.isSkillUsed = true;

        createTrans = Camera.main.transform;
        Vector3 createPos = createTrans.position + (createTrans.forward * 5f) + (createTrans.up * -1.5f);

        Skill skill = GameManager.Resource.Instantiate(skillData.skillPrefab, createPos, Quaternion.identity, true);

        skill.StartCoroutine(AttackJudgement(player, skill));
    }

    IEnumerator AttackJudgement(Player player, Skill skill)
    {
        float time = 0f;

        while (time < 3f)
        {
            time += Time.deltaTime;

            // Monster Binding & Damage
            Collider[] colliders = Physics.OverlapSphere(skill.transform.position, 3f, LayerMask.GetMask("Monster"));
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

        // Todo
        player.isSkillUsed = false;
        GameManager.Resource.Destroy(skill, 3f);
    }

    IEnumerator ActiveDamageTimer()
    {
        yield return new WaitForSeconds(1f);
        isDamaged = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 3f);
    }
}