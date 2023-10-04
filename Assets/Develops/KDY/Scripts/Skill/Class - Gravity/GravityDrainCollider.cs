using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GravityDrainCollider : MonoBehaviour
{
    public Skill skillSource;
    public Player source;

    private List<Vector3> dirList = new List<Vector3>()
    {
        Vector3.left, Vector3.right,
        (Vector3.up + Vector3.left) / 2, (Vector3.up + Vector3.right) / 2, (Vector3.down + Vector3.left) / 2, (Vector3.down + Vector3.right) / 2,
    };

    private Vector3 createPositon;

    private void OnCollisionEnter(Collision collision)
    {
        createPositon = GetCollisionTransform();

        if (collision.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            IHitReactor hitReactor = collision.gameObject.GetComponent<IHitReactor>();
            IHittable hitMonster = collision.gameObject.GetComponent<IHittable>();
            hitMonster.TakeDamaged(skillSource.skillData.damage);
            hitReactor.HitReact(skillSource.skillData.hitTags, 2f);
            StartCoroutine(PlayerSkillActiveRoutine(source, 2f));
            return;
        }

        DrainMonster();
    }

    private Vector3 GetCollisionTransform()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 5f))
        {
            return hitInfo.point;
        }

        return Vector3.zero;
    }

    private void DrainMonster()
    {
        Collider[] colliders = Physics.OverlapSphere(createPositon, 5f, LayerMask.GetMask("Monster"));

        foreach (Collider collider in colliders)
        {
            StartCoroutine(MonsterDrainRoutine(collider.transform));
        }
    }

    IEnumerator MonsterDrainRoutine(Transform monster)
    {
        CharacterController controller = monster.GetComponent<CharacterController>();

        if (controller != null)
            controller.enabled = false;

        Vector3 start = monster.transform.position;
        Vector3 end = createPositon;

        int randomDir = Random.Range(0, dirList.Count);
        float randomPos = Random.Range(1f, 4f);

        Vector3 center = ((start + end) / 2) + (dirList[randomDir] * randomPos);

        float time = 0f;
        float duration = Random.Range(1f, 2.5f);

        StartCoroutine(MonsterDamageRoutine(monster, duration));

        while (time < 1f)
        {
            Vector3 temp1 = Vector3.Lerp(start, center, time);
            Vector3 temp2 = Vector3.Lerp(center, end, time);
            monster.transform.position = Vector3.Lerp(temp1, temp2, time);

            time += Time.deltaTime / duration;

            yield return null;
        }

        source.isSkillUsed = false;

        if (controller != null)
            controller.enabled = true;

        if (skillSource.IsValid())
            GameManager.Resource.Destroy(skillSource);
    }

    IEnumerator MonsterDamageRoutine(Transform monster, float duration)
    {
        IHittable hitMonster = monster.GetComponent<IHittable>();

        float time = 0f;
        float hitIntervalTime = duration / 5f;

        while (time < duration)
        {
            time += Time.deltaTime;
            hitIntervalTime += Time.deltaTime;

            if (hitIntervalTime >= (duration / 5f))
            {
                hitMonster.TakeDamaged((skillSource.skillData.damage / 5) - 5);
                hitIntervalTime = 0f;
            }

            yield return null;
        }
    }

    IEnumerator PlayerSkillActiveRoutine(Player player, float duration)
    {
        yield return new WaitForSeconds(duration);

        player.isSkillUsed = false;
        if (skillSource.IsValid())
            GameManager.Resource.Destroy(skillSource);
    }

    private void OnDrawGizmos()
    {
        if (createPositon == Vector3.zero)
            return;

        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(createPositon, 5f);
    }
}