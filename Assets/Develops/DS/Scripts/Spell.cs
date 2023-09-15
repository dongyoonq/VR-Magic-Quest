using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumType;
using static MonsterSkillData;

public class Spell : MonoBehaviour
{
    private SpellHitbox spellHitbox;
    private Coroutine spellRoutine;
    private SkillInfo skillInfo;

    private void Awake()
    {
        spellHitbox = GetComponentInChildren<SpellHitbox>();
        spellHitbox.SynchronizeSpell(this);
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void Hit(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, skillInfo.hitRange);
        foreach (Collider collider in colliders)
        {
            IHittable hittable = collider.GetComponent<IHittable>();
            IHitReactor hitReactor = collider.GetComponent<IHitReactor>();
            hittable.TakeDamaged(skillInfo.damage);
            hitReactor.HitReact(skillInfo.hitType, 1f);
        }
    }

    public Vector3 SynchronizeSpell(SkillInfo skillInfo)
    {
        this.skillInfo = skillInfo;
        switch (skillInfo.spellType)
        {
            case SpellType.Area:
                spellRoutine = StartCoroutine(AreaSpellRoutine());
                break;
            case SpellType.Projectile:
                spellRoutine = StartCoroutine(ProjectileSpellRoutine());
                break;
            case SpellType.Falling:
                spellRoutine = StartCoroutine(FallingSpellRoutine());
                break;
            default:
                spellRoutine = StartCoroutine(BurstSpellRoutine());
                break;
        }
        return transform.position;
    }

    public IEnumerator AreaSpellRoutine()
    {
        yield return new WaitForSeconds (skillInfo.delayTime);
        float time = 0f;
        while (time < skillInfo.spellDuration)
        {
            time += Time.deltaTime;
            yield return null;
        }
        // 임시
        skillInfo.GetOtherPosition(transform.position);
        GameManager.Resource.Destroy(gameObject);
    }

    public IEnumerator ProjectileSpellRoutine()
    {
        yield return new WaitForSeconds(skillInfo.delayTime);
        float time = 0f;
        while (time < skillInfo.spellDuration)
        {
            time += Time.deltaTime;
            yield return null;
        }
        // 임시
        skillInfo.GetOtherPosition(transform.position);
        GameManager.Resource.Destroy(gameObject);
    }

    public IEnumerator FallingSpellRoutine()
    {
        yield return new WaitForSeconds(skillInfo.delayTime);
        float time = 0f;
        while (time < skillInfo.spellDuration)
        {
            time += Time.deltaTime;
            yield return null;
        }
        // 임시
        skillInfo.GetOtherPosition(transform.position);
        GameManager.Resource.Destroy(gameObject);
    }

    public IEnumerator BurstSpellRoutine()
    {
        yield return new WaitForSeconds(skillInfo.delayTime);
        float time = 0f;
        while (time < skillInfo.spellDuration)
        {
            time += Time.deltaTime;
            yield return null;
        }
        // 임시
        skillInfo.GetOtherPosition(transform.position);
        GameManager.Resource.Destroy(gameObject);
    }
}
