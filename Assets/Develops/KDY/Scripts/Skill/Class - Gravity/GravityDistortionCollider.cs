using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GravityDistortionCollider : MonoBehaviour
{
    public Skill skillSource;
    public Player source;

    private Vector3 createPositon;

    private void OnCollisionEnter(Collision collision)
    {
        createPositon = GetCollisionTransform();

        AttackJudgement();
    }

    private Vector3 GetCollisionTransform()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 5f))
        {
            return hitInfo.point;
        }

        return Vector3.zero;
    }

    private void AttackJudgement()
    {
        Collider[] colliders = Physics.OverlapSphere(createPositon, 5f, LayerMask.GetMask("Monster"));

        foreach (Collider collider in colliders)
        {
            collider.GetComponent<IHitReactor>().HitReact(skillSource.skillData.hitTags, 2f);
            StartCoroutine(RotateDamageRoutine(collider.transform));
        }
    }

    IEnumerator RotateDamageRoutine(Transform monster)
    {
        Vector3 startRotate = monster.rotation.eulerAngles;
        Vector3 endRotate = new Vector3(startRotate.x + 360f, startRotate.y, startRotate.z);

        float time = 0f;
        float duration = 2f;

        StartCoroutine(MonsterDamageRoutine(monster, duration));

        while (time < duration)
        {
            time += Time.deltaTime;

            float t = time / duration;
            Vector3 currentRotation = Vector3.Lerp(startRotate, endRotate, t);

            monster.rotation = Quaternion.Euler(currentRotation);

            yield return null;
        }

        source.isSkillUsed = false;

        if (skillSource.IsValid())
            GameManager.Resource.Destroy(skillSource);
    }

    IEnumerator MonsterDamageRoutine(Transform monster, float duration)
    {
        IHittable hitMonster = monster.GetComponent<IHittable>();

        float time = 0f;
        float hitIntervalTime = duration / 4f;

        while (time < duration)
        {
            time += Time.deltaTime;
            hitIntervalTime += Time.deltaTime;

            if (hitIntervalTime >= (duration / 4f))
            {
                hitMonster.TakeDamaged(skillSource.skillData.damage);
                hitIntervalTime = 0f;
            }

            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        if (createPositon == Vector3.zero)
            return;

        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(createPositon, 5f);
    }
}