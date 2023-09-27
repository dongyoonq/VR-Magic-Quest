using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class LightningChargeRayCast : MonoBehaviour
{
    public Skill source;
    public Player player;
    public bool isHitted = false;

    private int rayDistance = 10;
    private float activeHitTime = 1f;

    private void Update()
    {
        if (!isHitted && Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, rayDistance))
        {
            IHittable hitMonster = hitInfo.collider.GetComponent<IHittable>();
            IHitReactor hitReactor = hitInfo.collider.GetComponent<IHitReactor>();

            if (hitMonster != null)
            {
                isHitted = true;
                hitMonster.TakeDamaged(source.skillData.damage);
                hitReactor.HitReact(source.skillData.hitTags, 0.3f);
                StartCoroutine(HitActiveRoutine());
            }
        }
    }

    IEnumerator HitActiveRoutine()
    {
        yield return new WaitForSeconds(activeHitTime);
        isHitted = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * rayDistance);
    }
}