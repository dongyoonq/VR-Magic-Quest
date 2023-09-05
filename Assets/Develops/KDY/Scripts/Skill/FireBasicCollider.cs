using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FireBasicCollider : MonoBehaviour
{
    public SkillData skillData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            IHittable hitMonster = other.gameObject.GetComponent<IHittable>();
            hitMonster.TakeDamaged(skillData.damage);

            if (transform.parent.IsValid())
                GameManager.Resource.Destroy(transform.parent.gameObject);
        }
    }
}