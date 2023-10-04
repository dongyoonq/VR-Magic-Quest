using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SkillProjectileRaycast : MonoBehaviour
{
    public Skill source;
    public Player player;

    public float hitTime;

    private float time = 0f;
    private float remainTime = 0f;
    private bool isHitted = false;

    private void OnEnable()
    {
        isHitted = false;
        time = 0f;
        remainTime = 0f;
    }

    private void Update()
    {
        time += Time.deltaTime;

        if (time >= 5f)
        {
            player.isSkillUsed = false;

            if (source.IsValid())
                GameManager.Resource.Destroy(source);
        }

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 100) && !isHitted)
        {
            IHittable hitMonster = hitInfo.collider.GetComponent<IHittable>();
            IHitReactor hitReactor = hitInfo.collider.GetComponent<IHitReactor>();

            if (hitMonster != null)
            {
                isHitted = true;
                remainTime = 5f - time;

                StartCoroutine(DamageRoutine(hitMonster));
                StartCoroutine(ReactRoutine(hitReactor));
                StartCoroutine(EndSkillRoutine());
            }
        }
    }

    IEnumerator DamageRoutine(IHittable hitMonster)
    {
        yield return new WaitForSeconds(hitTime);
        hitMonster.TakeDamaged(source.skillData.damage);
    }

    IEnumerator ReactRoutine(IHitReactor hitReactor)
    {
        yield return new WaitForSeconds(hitTime);
        hitReactor.HitReact(source.skillData.hitTags, 0.4f);
    }

    IEnumerator EndSkillRoutine()
    {
        yield return new WaitForSeconds(remainTime);
        player.isSkillUsed = false;

        if (source.IsValid())
            GameManager.Resource.Destroy(source);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 100);
    }
}