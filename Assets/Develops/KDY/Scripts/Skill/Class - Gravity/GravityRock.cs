using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityRock : Skill
{
    public override void CastingSpell(Player player, float value, Transform createTrans)
    {
        player.isSkillUsed = true;

        createTrans = Camera.main.transform;
        Vector3 createPos = createTrans.position + (createTrans.forward * 4f) + (createTrans.right * -2f);
        Skill skill = GameManager.Resource.Instantiate(skillData.skillPrefab, createPos, Quaternion.LookRotation(createTrans.forward), true);
        skill.StartCoroutine(RockJudgementRoutine(player, skill));
    }

    IEnumerator RockJudgementRoutine(Player player, Skill skill)
    {
        yield return new WaitForSeconds(1f);

        Vector3 center = skill.transform.position + (skill.transform.right * 2.2f) + (skill.transform.forward * -0.5f) + (skill.transform.up * -1f);
        float radius = 1.8f;

        Collider[] colliders = Physics.OverlapSphere(center, radius, LayerMask.GetMask("Monster"));
        foreach (Collider collider in colliders)
        {
            IHittable hitMonster = collider.GetComponent<IHittable>();
            hitMonster.TakeDamaged(skill.skillData.damage);
        }

        yield return new WaitForSeconds(2.5f);

        player.isSkillUsed = false;
        GameManager.Resource.Destroy(skill);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (transform.right * 2.2f) + (transform.forward * -0.5f) + (transform.up * -1f), 1.8f);
    }
}