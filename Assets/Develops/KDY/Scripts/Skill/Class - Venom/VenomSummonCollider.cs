using System.Collections;
using UnityEngine;

public class VenomSummonCollider : MonoBehaviour
{
    public VenomSummon skillSource;
    public Player source;

    const string instGo = "Effect17_Collision";
    bool isCollision = false;
    bool isReady = false;

    private Vector3 createPositon;
    private Vector3 hitPoint;

    private void OnEnable()
    {
        isCollision = false;
        isReady = false;

        StartCoroutine(StartUpdate());
    }

    IEnumerator StartUpdate()
    {
        yield return new WaitForSeconds(0.8f);
        isReady = true;
    }

    private void Update()
    {
        if (!isCollision && isReady)
        {
            CollisionDamage();
        }
    }

    private void CollisionDamage()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 0.5f))
        {
            isCollision = true;
            hitPoint = hitInfo.point;
            Collider[] colliders = Physics.OverlapSphere(hitInfo.point, 0.5f);

            foreach (Collider collider in colliders)
            {
                IHittable hitMonster = collider.GetComponent<IHittable>();
                IHitReactor hitReactor = collider.gameObject.GetComponent<IHitReactor>();

                if (hitMonster != null)
                {
                    hitMonster.TakeDamaged(skillSource.skillData.damage);
                }

                if (hitReactor != null)
                {
                    hitReactor.HitReact(skillSource.skillData.hitTags, 0.5f);
                }
            }

            StartCoroutine(AfterMathJudgement());
        }
    }

    IEnumerator AfterMathJudgement()
    {
        yield return new WaitForSeconds(1.5f);

        GameObject instObj = GameObject.Find(instGo + "(Clone)");

        if (instObj != null)
        {
            createPositon = instObj.transform.position;
        }

        if (skillSource.isCollision)
            yield break;

        Collider[] colliders = Physics.OverlapSphere(createPositon, 5f, LayerMask.GetMask("Monster"));

        foreach (Collider collider in colliders)
        {
            IHittable hitMonster = collider.gameObject.GetComponent<IHittable>();
            hitMonster.TakeDamaged(skillSource.skillData.damage / 2);
        }

        skillSource.isCollision = true;

        yield return new WaitForSeconds(2f);

        source.isSkillUsed = false;

        if (skillSource.IsValid())
            GameManager.Resource.Destroy(skillSource.gameObject);
    }

    private void OnDrawGizmos()
    {
        if (hitPoint == Vector3.zero)
            return;

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(createPositon, 0.5f);

        if (createPositon == Vector3.zero)
            return;

        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(createPositon, 5f);
    }
}