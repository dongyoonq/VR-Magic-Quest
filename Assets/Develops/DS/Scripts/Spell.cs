using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumType;
using static MonsterSkillData;

public class Spell : MonoBehaviour
{
    private SpellHitbox spellHitbox;
    public SpellHitbox SpellHitbox { get { return spellHitbox; } }
    private SkillInfo skillInfo;
    private Spell previousSpell;
    public Spell PreviousSpell { get { return previousSpell; } set {  previousSpell = value; } }
    private Transform casterTransform;
    public Transform CasterTransform { get { return casterTransform; } }
    private float casterAttackPoint;
    public bool activate;
    private GameObject effect;
    private GameObject skillRangeDecal;
    private float time = 0f;

    private void Awake()
    {
        effect = transform.GetChild(0).gameObject;
        if (transform.childCount > 1)
        {
            skillRangeDecal = transform.GetChild(1).gameObject;
        }
        effect.SetActive(true);
        skillRangeDecal?.SetActive(false);
        spellHitbox = GetComponentInChildren<SpellHitbox>();
        spellHitbox?.SynchronizeSpell(this);
        effect.SetActive(false);
    }

    private void OnEnable()
    {
        skillInfo = null;
        casterTransform = null;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void SynchronizeSpell(SkillInfo skillInfo, Transform casterTransform, int casterAttackPoint)
    {
        this.skillInfo = skillInfo;
        this.casterTransform = casterTransform;
        this.casterAttackPoint = casterAttackPoint;
        StartCoroutine(SpellRoutine());
    }

    private IEnumerator SpellRoutine()
    {
        if (skillInfo.activateTiming == ActivateTiming.After)
        {
            yield return new WaitForSeconds(skillInfo.delayTime);
            skillRangeDecal?.gameObject.SetActive(true);
            yield return new WaitWhile(() => previousSpell.activate);
            if (skillInfo.aim == Aim.Target)
            {
                transform.position = previousSpell.spellHitbox.transform.position;
            }
        }
        else
        {
            skillRangeDecal?.gameObject.SetActive(true);
            yield return new WaitForSeconds(skillInfo.delayTime);
        }        
        skillRangeDecal?.gameObject.SetActive(false);
        activate = true;
        effect.SetActive(true);
        if (skillInfo.spellType == SpellType.Burst)
        {
            Hit(transform.position);
        }
        yield return new WaitForSeconds(skillInfo.spellDuration);
        activate = false;
        effect.SetActive(false);
        GameManager.Resource.Destroy(gameObject);
    }

    public void Hit(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, skillInfo.hitRange);
        foreach (Collider collider in colliders)
        {
            if (skillInfo.influenceSelf)
            {
                if (collider.transform == casterTransform)
                {
                    IHittable hittable = collider.GetComponent<IHittable>();
                    IHitReactor hitReactor = collider.GetComponent<IHitReactor>();
                    hittable?.TakeDamaged(Mathf.FloorToInt(casterAttackPoint * skillInfo.damageMultiplier));
                    hitReactor?.HitReact(skillInfo.hitType, skillInfo.spellDuration - 1f);
                }
            }
            else
            {
                if (collider.transform != casterTransform)
                {
                    IHittable hittable = collider.GetComponent<IHittable>();
                    IHitReactor hitReactor = collider.GetComponent<IHitReactor>();
                    hittable?.TakeDamaged(Mathf.FloorToInt(casterAttackPoint * skillInfo.damageMultiplier));
                    hitReactor?.HitReact(skillInfo.hitType, skillInfo.spellDuration - 1f);
                }
            }
        }
    }

    public void ProjectileHit(Vector3 position)
    {
        if (skillInfo.spellType == SpellType.Projectile)
        {
            Hit(position);
        }
    }
    
    public void ContinuousHit(Collider collider)
    {
        if (skillInfo.spellType == SpellType.Area)
        {
            if (time >= 0.5f)
            {
                if (skillInfo.influenceSelf)
                {
                    if (collider.transform == casterTransform)
                    {
                        IHittable hittable = collider.GetComponent<IHittable>();
                        IHitReactor hitReactor = collider.GetComponent<IHitReactor>();
                        hittable?.TakeDamaged(Mathf.FloorToInt(casterAttackPoint * skillInfo.damageMultiplier));
                        hitReactor?.HitReact(skillInfo.hitType, skillInfo.spellDuration - 1f);
                    }
                }
                else
                {
                    if (collider.transform != casterTransform)
                    {
                        IHittable hittable = collider.GetComponent<IHittable>();
                        IHitReactor hitReactor = collider.GetComponent<IHitReactor>();
                        hittable?.TakeDamaged(Mathf.FloorToInt(casterAttackPoint * skillInfo.damageMultiplier));
                        hitReactor?.HitReact(skillInfo.hitType, skillInfo.spellDuration - 1f);
                    }
                }
                time = 0f;
            }
            else
            {
                time += Time.deltaTime;
            }
        }
    }
}
