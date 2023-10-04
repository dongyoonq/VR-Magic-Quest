using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using static MonsterCombat;

public class FireBoltex : Skill
{
    private int reachCount;
    private const int createCount = 8;

    private List<Vector3> dirList = new List<Vector3>()
    {
        Vector3.up, Vector3.down, Vector3.left, Vector3.right,
        (Vector3.up + Vector3.left) / 2, (Vector3.up + Vector3.right) / 2, (Vector3.down + Vector3.left) / 2, (Vector3.down + Vector3.right) / 2,
    };

    public override void CastingSpell(Player player, float value, Transform createTrans)
    {
        reachCount = 0;

        player.isSkillUsed = true;

        createTrans = Camera.main.transform;
        Vector3 createPos = createTrans.position + (createTrans.forward * 2f);

        player.StartCoroutine(PlayerSkillActive(player));

        for (int i = 0; i < createCount; i++)
        {
            Skill skill = GameManager.Resource.Instantiate(skillData.skillPrefab, createPos, Quaternion.LookRotation(createTrans.forward), true);
            skill.StartCoroutine(BezierCurveRoutine(skill));
        }
    }

    IEnumerator BezierCurveRoutine(Skill skill)
    {
        Vector3 start = skill.transform.position;
        Vector3 end = skill.transform.position + skill.transform.forward * 8f;

        int randomDir = Random.Range(0, dirList.Count);
        float randomPos = Random.Range(3f, 6f);

        Vector3 center = ((start + end) / 2) + (dirList[randomDir] * randomPos);

        float time = 0f;
        float duration = Random.Range(1f, 2.5f);

        while (time < 1f)
        {
            Vector3 temp1 = Vector3.Lerp(start, center, time);
            Vector3 temp2 = Vector3.Lerp(center, end, time);
            skill.transform.position = Vector3.Lerp(temp1, temp2, time);

            time += Time.deltaTime / duration;

            yield return null;
        }

        reachCount++;

        if (skill.IsValid())
            GameManager.Resource.Destroy(skill);
    }

    IEnumerator PlayerSkillActive(Player player)
    {
        while (true)
        {
            if (reachCount == createCount)
            {
                player.isSkillUsed = false;
                yield break;
            }

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            IHittable hitMonster = other.gameObject.GetComponent<IHittable>();
            IHitReactor hitReactor = other.gameObject.GetComponent<IHitReactor>();
            hitReactor.HitReact(skillData.hitTags, 0.2f);
            hitMonster.TakeDamaged(skillData.damage);
        }
    }
}