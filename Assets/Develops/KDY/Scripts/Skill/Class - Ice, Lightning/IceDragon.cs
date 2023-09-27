using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class IceDragon : Skill
{
    public override void CastingSpell(Player player, float correctValue, Transform createTrans)
    {
        player.isSkillUsed = true;

        createTrans = Camera.main.transform;

        Vector3 createPos;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hitInfo, 6f))
        {
            createPos = hitInfo.point + (hitInfo.normal) + (createTrans.forward * 4f);
        }
        else
        {
            createPos = createTrans.position + (createTrans.forward * 5f) + (createTrans.up * -1f);
        }

        Skill skill = GameManager.Resource.Instantiate(skillData.skillPrefab, createPos, Quaternion.identity, true);

        skill.StartCoroutine(AttackJudgement(player, skill));
    }

    IEnumerator AttackJudgement(Player player, Skill skill)
    {
        yield return new WaitForSeconds(3.5f);

        Vector3 ceneter = skill.transform.position + (skill.transform.up * 3.5f);
        Vector3 boxSzie = new Vector3(2.3f, 6.3f, 2.3f);
        Collider[] colliders = Physics.OverlapBox(ceneter, boxSzie / 2f, Quaternion.identity, LayerMask.GetMask("Monster"));

        foreach (Collider collider in colliders)
        {
            IHittable hitMonster = collider.GetComponent<IHittable>();
            IHitReactor hitReactor = collider.GetComponent<IHitReactor>();
            hitReactor.HitReact(skillData.hitTags, 0.3f);
            hitMonster.TakeDamaged(skill.skillData.damage);
        }

        yield return new WaitForSeconds(2.5f);

        player.isSkillUsed = false;
        GameManager.Resource.Destroy(skill, 2f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + (transform.up * 3.5f), new Vector3(2.3f, 6.3f, 2.3f));
    }
}