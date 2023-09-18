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
    public bool activate;
    private GameObject effect;
    private Coroutine spellRoutine;

    private void Awake()
    {
        effect = transform.GetChild(0).gameObject;
        effect.SetActive(true);
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

    public void SynchronizeSpell(SkillInfo skillInfo, Transform casterTransform)
    {
        this.skillInfo = skillInfo;
        this.casterTransform = casterTransform;
        spellRoutine = StartCoroutine(SpellRoutine());
    }

    private IEnumerator SpellRoutine()
    {
        activate = true;
        if (skillInfo.activateTiming == ActivateTiming.After)
        {
            yield return new WaitWhile(() => previousSpell.activate);
            if (skillInfo.aim == Aim.Target)
            {
                transform.position = previousSpell.spellHitbox.transform.position;
            }
        }
        yield return new WaitForSeconds(skillInfo.delayTime);
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
            if (collider.transform != casterTransform)
            {
                IHittable hittable = collider.GetComponent<IHittable>();
                IHitReactor hitReactor = collider.GetComponent<IHitReactor>();
                hittable?.TakeDamaged(skillInfo.damage);
                hitReactor?.HitReact(skillInfo.hitType, 1f);
            }
        }
    }
}
