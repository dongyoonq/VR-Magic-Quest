using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class VenomStrikeCollider : MonoBehaviour
{
    public Skill skillSource;
    public Player source;

    private Vector3 createPositon;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            IHittable hitMonster = collision.gameObject.GetComponent<IHittable>();
            hitMonster.TakeDamaged(skillSource.skillData.damage);
        }

        createPositon = GetCollisionTransform();
        StartCoroutine(AfterMathJudgement());
    }

    private Vector3 GetCollisionTransform()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 15f))
        {
            return hitInfo.point;
        }

        return Vector3.zero;
    }

    IEnumerator AfterMathJudgement()
    {
        yield return new WaitForSeconds(0.4f);

        Collider[] colliders = Physics.OverlapSphere(createPositon, 3f, LayerMask.GetMask("Monster"));

        foreach (Collider collider in colliders)
        {
            IHittable hitMonster = collider.gameObject.GetComponent<IHittable>();
            IHitReactor hitReactor = collider.gameObject.GetComponent<IHitReactor>();
            hitReactor.HitReact(skillSource.skillData.hitTags, 0.5f);
            hitMonster.TakeDamaged(skillSource.skillData.damage / 2);
        }

        yield return new WaitForSeconds(2f);

        source.isSkillUsed = false;

        if (skillSource.IsValid())
            GameManager.Resource.Destroy(skillSource.gameObject);
    }

    private void OnDrawGizmos()
    {
        if (createPositon == Vector3.zero)
            return;

        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(createPositon, 3f);
    }
}