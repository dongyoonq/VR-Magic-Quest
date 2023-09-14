using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class VenomBinding : Skill
{
    private bool isDamaged;

    public override void CastingSpell(Player player, float correctValue, Transform createTrans)
    {
        isDamaged = false;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 12, LayerMask.GetMask("Monster")))
        {
            Vector3 createPos = hit.point + (hit.transform.up * -1f);
            Skill skill = GameManager.Resource.Instantiate(skillData.skillPrefab, createPos, Quaternion.identity, true);

            player.isSkillUsed = true;

            skill.StartCoroutine(AttackJudgement(skill));
            skill.StartCoroutine(BindingEndRoutine(player, skill));
        }
    }

    IEnumerator AttackJudgement(Skill skill)
    {
        float time = 0f;

        Vector3 ceneter = skill.transform.position;
        Vector3 boxSzie = new Vector3(2f, 3.5f, 2f);
        Collider[] colliders = Physics.OverlapBox(ceneter, boxSzie / 2f, Quaternion.identity, LayerMask.GetMask("Monster"));

        foreach (Collider collider in colliders)
        {
            IHitReactor hitReactor = collider.GetComponent<IHitReactor>();

            if (hitReactor != null)
                hitReactor.HitReact(skillData.hitTags, 5f);
        }

        while (time < 5f)
        {
            time += Time.deltaTime;

            // Monster Binding & Damage
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
    }

    IEnumerator BindingEndRoutine(Player player, Skill skill)
    {
        yield return new WaitForSeconds(2f);

        player.isSkillUsed = false;

        yield return new WaitForSeconds(3f);

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
        Gizmos.DrawWireCube(transform.position, new Vector3(2f, 3.5f, 2f));
    }
}