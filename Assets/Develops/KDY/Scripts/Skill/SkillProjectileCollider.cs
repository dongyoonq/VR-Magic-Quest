using System.Collections;
using UnityEngine;

public class SkillProjectileCollider : MonoBehaviour
{
    public Skill skillSource;
    public Player source;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            IHittable hitMonster = collision.gameObject.GetComponent<IHittable>();
            hitMonster.TakeDamaged(skillSource.skillData.damage);
            StartCoroutine(PlayerSkillActiveRoutine(source, 2f));
        }
    }

    IEnumerator PlayerSkillActiveRoutine(Player player, float duration)
    {
        yield return new WaitForSeconds(duration);

        player.isSkillUsed = false;
        if (skillSource.IsValid())
            GameManager.Resource.Destroy(skillSource);
    }
}