using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using static EnumType;
using static MonsterCombat;
using static MonsterSkillData;

public class MonsterCombat : MonoBehaviour, IHitReactor, IHittable
{
    private MonsterPerception perception;
    public Animator animator;
    [SerializeField]
    private (int healthPoint, int attackPoint, float attackSpeed) stat;
    public (int healthPoint, int attackPoint, float attackSpeed) Stat { get { return stat; } set { stat = value; } }
    private int maxHP;
    public int MaxHP { get { return maxHP; } set { maxHP = value; } }
    private WaitForSeconds waitRecoverTime;
    public WaitForSeconds WaitRecoverTime { get { return waitRecoverTime; } set { waitRecoverTime = value; } }
    private float statusDuration;
    private LayerMask hitTargetLayerMask;
    private Coroutine attackRoutine;
    private Coroutine concentrateRoutine;
    private bool getHit;
    [SerializeField]
    private MonsterSkillData skillData;
    [SerializeField]
    private GameObject concentrateObject;
    private bool channelling;
    private bool invincible;
    [SerializeField]
    private Skill[] attackPatern;
    [SerializeField]
    private Skill[] conditionalPatern;
    private List<Skill> skillPriority;
    private float delayTime;
    public bool rageMode;
    [SerializeField]
    private HitTag[] basicAttackType;
    [SerializeField]
    private string attackSound;
    [SerializeField]
    private string getDamageSound;

    [Serializable]
    public class Skill
    {
        public MonsterSkill skill;
        public int energyCost;
        public int limit;
        public int castingMotion;
        public int conditionHP;
        public float conditionDistance;
        [HideInInspector]
        public int priority = 0;
    }

    private void Awake()
    {
        perception = GetComponent<MonsterPerception>();
        animator = GetComponent<Animator>();
        hitTargetLayerMask = LayerMask.GetMask("Player");
        concentrateObject = GameManager.Resource.Instantiate(concentrateObject, transform.position + Vector3.up * 2f, transform.rotation);
        concentrateObject.transform.SetParent(transform);
        concentrateObject.SetActive(false);
    }

    private void OnEnable()
    {
        getHit = false;
        channelling = false;
        delayTime = 2f;
        skillPriority = new List<Skill>();
        foreach (Skill skill in attackPatern)
        {
            skillPriority.Add(skill);
            skill.priority++;
        }
        //StartCoroutine(Test());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator Test()
    {
        yield return new WaitForSeconds(10f);
        TakeDamaged(100000);
    }

    //TODO: 공격 딜레이 타임 조정, 돌진, 회피
    // advanced state에 따라 다른 공격
    public void Combat()
    {
        delayTime += -Time.deltaTime;
        if (delayTime <= 0f)
        {
            delayTime = UnityEngine.Random.Range(5, 10);
            if (attackSound != null)
            {
                GameManager.Sound.PlaySFX(attackSound);
            }            
            for (int i = 0; i < conditionalPatern.Length; i++)
            {
                if (stat.healthPoint < conditionalPatern[i].conditionHP)
                {
                    if (conditionalPatern[i].limit == 0)
                    {
                        continue;
                    }
                    Cast(ref conditionalPatern[i]);
                    conditionalPatern[i].limit--;
                    return;
                }
                if (conditionalPatern[i].conditionDistance != 0f)
                {
                    if (!perception.CompareDistanceWithoutHeight(transform.position, perception.Vision.TargetTransform.position, conditionalPatern[i].conditionDistance))
                    {
                        if (conditionalPatern[i].limit == 0)
                        {
                            continue;
                        }
                        Cast(ref conditionalPatern[i]);
                        conditionalPatern[i].limit--;
                        return;
                    }
                }             
            }
            Skill skill = skillPriority[UnityEngine.Random.Range(0, skillPriority.Count)];
            for (int i = 0; i < skill.priority; i++)
            {
                skillPriority.Remove(skill);
            }
            skill.priority = 0;
            foreach (Skill patern in attackPatern)
            {
                if (patern.limit != 0)
                {
                    skillPriority.Add(patern);
                    patern.priority++;
                }
            }
            Cast(ref skill);
            skill.limit--;
            if (skill.limit == 0)
            {
                skillPriority.Remove(skill);
            }    
        }
    }

    private void CountDown(ref float time)
    {
        while (time <= 0)
        {
            time += -Time.deltaTime;
        }
    }

    private void Attack(int damage, HitTag[] attackType)
    {
        Collider[] hitObjects = Physics.OverlapSphere(Camera.main.transform.position, 1f, hitTargetLayerMask);
        if (perception.CurrentCondition == Condition.TopForm)
        {
            damage += damage / 2;
        }
        foreach (Collider hitObject in hitObjects)
        {
            hitObject.GetComponent<IHittable>()?.TakeDamaged(damage);
            hitObject.GetComponent<IHitReactor>()?.HitReact(attackType, 1f);
        }
    }

    public (bool, int) CheckCondition(Skill[] skillGroup)
    {
        bool usable = false;
        int index = 0;
        Condition expectedCondition = Condition.TopForm + 1;
        for (int i = 0; i < skillGroup.Length; i++)
        {
            if (skillGroup[i].limit == 0)
            {
                continue;
            }
            if (perception.CurrentCondition - skillGroup[i].energyCost >= Condition.Weak)
            {
                usable = true;
                if (perception.CurrentCondition - skillGroup[i].energyCost < expectedCondition)
                {
                    expectedCondition = perception.CurrentCondition - skillGroup[i].energyCost;
                    index = i;
                }
            }
        }
        return (usable, index);
    }

    public void CheckConditionAndUse(Skill[] skillGroup)
    {
        bool usable = false;
        int index = 0;
        Condition expectedCondition = Condition.TopForm + 1;
        for (int i = 0; i < skillGroup.Length; i++)
        {
            if (skillGroup[i].limit == 0)
            {
                continue;
            }
            if (perception.CurrentCondition - skillGroup[i].energyCost >= Condition.Weak)
            {
                usable = true;
                if (perception.CurrentCondition - skillGroup[i].energyCost < expectedCondition)
                {
                    expectedCondition = perception.CurrentCondition - skillGroup[i].energyCost;
                    index = i;
                }
            }
        }
        if (usable)
        {
            Cast(ref skillGroup[index]);
        }
    }

    private IEnumerator BasicAttackRoutine()
    {
        animator.SetTrigger("Attack");
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsTag("AttackFinish"));
        Attack(stat.attackPoint, basicAttackType);
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).IsTag("AttackFinish"));
    }

    public void Rage()
    {
        MonsterSkillData.SkillInfo skillInfo = skillData.GetSkillInfo(MonsterSkill.Rage);
        concentrateRoutine = StartCoroutine(CastSpellRoutine(skillInfo));
        perception.SendCommand(ConcentrateRoutine(true));
    }

    public void Cast(ref Skill skill)
    {
        if (skill.skill == MonsterSkill.Basic)
        {
            perception.SendCommand(BasicAttackRoutine());
            delayTime -= delayTime * stat.attackSpeed;
        }
        else
        {
            perception.ChangeCondition(-skill.energyCost);
            perception.SendCommand(CastRoutine(skill));
        }
    }

    private IEnumerator CastRoutine(Skill skill)
    {
        yield return null;
        MonsterSkillData.SkillInfo skillInfo = skillData.GetSkillInfo(skill.skill);
        concentrateRoutine = StartCoroutine(CastSpellRoutine(skillInfo));
        if (skillInfo.castingTime == 0)
        {
            perception.SendCommand(ConcentrateRoutine(true));
        }
        else
        {
            perception.SendCommand(ConcentrateRoutine(skillInfo.castingTime, skill.castingMotion));
        }
    }

    private IEnumerator CastSpellRoutine(MonsterSkillData.SkillInfo skillInfo)
    {
        channelling = true;
        yield return new WaitForSeconds(skillInfo.castingTime);
        Vector3 targetPosition = perception.Vision.TargetTransform.position;
        if (rageMode)
        {
            switch (skillInfo.aim)
            {
                case Aim.Target:
                    for (int i = 0; i < UnityEngine.Random.Range(1, 11); i++)
                    {
                        Vector3 randomPosition = UnityEngine.Random.insideUnitSphere * 20f + targetPosition;
                        ActivateSpell(skillInfo, perception.Vision.TargetTransform, new Vector3(randomPosition.x, targetPosition.y, randomPosition.z));
                    }
                    break;
                case Aim.Front:
                    for (int i = 1; i <= UnityEngine.Random.Range(1, 4); i++)
                    {
                        ActivateSpell(skillInfo, perception.Vision.TargetTransform, transform.position + transform.forward * 1.5f + transform.right * 1.5f * i);
                        ActivateSpell(skillInfo, perception.Vision.TargetTransform, transform.position + transform.forward * 1.5f + -transform.right * 1.5f * i);
                    }
                    break;
            }
        }
        ActivateSpell(skillInfo, perception.Vision.TargetTransform);
        channelling = false;
        animator.SetBool("Casting", false);
    }

    private void ActivateSpell(MonsterSkillData.SkillInfo skillInfo, Transform targetTransform)
    {
        Spell spell;
        GameObject skillPrefab;
        if (skillInfo.skillPrefab == null)
        {
            skillPrefab = skillData.defaultSkillPrefab;
        }
        else
        {
            skillPrefab = skillInfo.skillPrefab;
        }
        switch (skillInfo.aim)
        {
            case Aim.Target:                   
                spell = GameManager.Resource.Instantiate(skillPrefab, targetTransform.position, targetTransform.rotation, true)
                    .GetComponent<Spell>();
                spell.PreviousSpell = targetTransform.GetComponent<Spell>();
                spell.SynchronizeSpell(skillInfo, transform, stat.attackPoint);

                break;
            case Aim.Front:
                spell = GameManager.Resource.Instantiate
                    (skillPrefab, transform.position + transform.forward * 1.5f, transform.rotation, true).GetComponent<Spell>();
                spell.PreviousSpell = targetTransform.GetComponent<Spell>();
                spell.SynchronizeSpell(skillInfo, transform, stat.attackPoint);
                break;
            case Aim.Around:
                Vector3 aroundPosition = UnityEngine.Random.insideUnitSphere * 2f + targetTransform.position;
                spell = GameManager.Resource.Instantiate
                    (skillPrefab, new Vector3(aroundPosition.x, targetTransform.position.y, aroundPosition.z), targetTransform.rotation, true)
                   .GetComponent<Spell>();
                spell.PreviousSpell = targetTransform.GetComponent<Spell>();
                spell.SynchronizeSpell(skillInfo, transform, stat.attackPoint);
                break;
            default:
                spell = GameManager.Resource.Instantiate(skillPrefab, transform.position, transform.rotation, true)
                    .GetComponent<Spell>();
                spell.PreviousSpell = targetTransform.GetComponent<Spell>();
                spell.SynchronizeSpell(skillInfo, transform, stat.attackPoint);
                break;
        }
        if (skillInfo.additionalSkills.Length > 0)
        {
            for (int i = 0; i < skillInfo.additionalSkills.Length; i++)
            {
                ActivateSpell(skillInfo.additionalSkills[i], spell.transform);
            }
        }    
    }

    private void ActivateSpell(MonsterSkillData.SkillInfo skillInfo, Transform targetTransform, Vector3 targetPosition)
    {
        Spell spell;
        GameObject skillPrefab;
        if (skillInfo.skillPrefab == null)
        {
            skillPrefab = skillData.defaultSkillPrefab;
        }
        else
        {
            skillPrefab = skillInfo.skillPrefab;
        }
        switch (skillInfo.aim)
        {
            case Aim.Target:
                spell = GameManager.Resource.Instantiate(skillPrefab, targetPosition, targetTransform.rotation, true)
                    .GetComponent<Spell>();
                spell.PreviousSpell = targetTransform.GetComponent<Spell>();
                spell.SynchronizeSpell(skillInfo, transform, stat.attackPoint);

                break;
            case Aim.Front:
                spell = GameManager.Resource.Instantiate
                    (skillPrefab, targetPosition, transform.rotation, true).GetComponent<Spell>();
                spell.PreviousSpell = targetTransform.GetComponent<Spell>();
                spell.SynchronizeSpell(skillInfo, transform, stat.attackPoint);
                break;
            case Aim.Around:
                Vector3 aroundPosition = UnityEngine.Random.insideUnitSphere * 2f + targetTransform.position;
                spell = GameManager.Resource.Instantiate
                    (skillPrefab, new Vector3(aroundPosition.x, targetTransform.position.y, aroundPosition.z), targetTransform.rotation, true)
                   .GetComponent<Spell>();
                spell.PreviousSpell = targetTransform.GetComponent<Spell>();
                spell.SynchronizeSpell(skillInfo, transform, stat.attackPoint);
                break;
            default:
                spell = GameManager.Resource.Instantiate(skillPrefab, transform.position, transform.rotation, true)
                    .GetComponent<Spell>();
                spell.PreviousSpell = targetTransform.GetComponent<Spell>();
                spell.SynchronizeSpell(skillInfo, transform, stat.attackPoint);
                break;
        }
        if (skillInfo.additionalSkills.Length > 0)
        {
            for (int i = 0; i < skillInfo.additionalSkills.Length; i++)
            {
                ActivateSpell(skillInfo.additionalSkills[i], spell.transform);
            }
        }
    }

    private IEnumerator ConcentrateRoutine(float castingTime, int castingMotion)
    {
        if (castingMotion == 0)
        {
            animator.SetBool("Casting", true);
            animator.SetFloat("CastingTime", 1 / castingTime);
        }
        else
        {
            animator.SetInteger("SpecialAttack", castingMotion);
        }
        concentrateObject.SetActive(true);
        yield return new WaitWhile(() => channelling);
        concentrateObject.SetActive(false);
        animator.SetFloat("CastingTime", 1f);
        animator.SetInteger("SpecialAttack", 0);
    }

    private IEnumerator ConcentrateRoutine(bool burst)
    {
        yield return new WaitWhile(() => channelling);
    }

    public void Meditation()
    {
        channelling = true;
        concentrateRoutine = StartCoroutine(MeditationRoutine());
        perception.SendCommand(ConcentrateRoutine(3f, 0));
    }

    private IEnumerator MeditationRoutine()
    {
        int time = 0;
        while(perception.CurrentCondition <= Condition.TopForm && time <= 3)
        {
            yield return new WaitForSeconds(1f);
            time++;
            perception.ChangeCondition(2);
        }
        channelling = false;
    }

    public void HitReact(HitTag[] hitType, float duration)
    {
        getHit = true;
        foreach (HitTag hitTag in hitType)
        {
            statusDuration = duration;
            if (invincible)
            {
                return;
            }
            switch (hitTag)
            {
                case HitTag.Impact:
                    perception.SendCommand(ImpactHitReactRoutine());
                    break;
                case HitTag.Debuff:
                    perception.SendCommand(DeBuffHitReactRoutine());
                    break;
                case HitTag.Buff:
                    perception.SendCommand(BuffHitReactRoutine());
                    break;
                case HitTag.Mez:
                    perception.SendCommand(MezHitReactRoutine());
                    break;
                case HitTag.Invincible:
                    perception.SendCommand(InvincibleHitReactRoutine());
                    break;
                case HitTag.Rage:
                    perception.SendCommand(RageHitReactRoutine());
                    break;
            }
        }
    }

    private IEnumerator ImpactHitReactRoutine()
    {
        yield return null;
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            if (channelling)
            {
                perception.ChangeCondition(-1);
                animator.SetBool("HeavyAttack", false);
                channelling = false;
            }            
        }
        if (concentrateRoutine != null)
        {
            StopCoroutine(concentrateRoutine);
            animator.SetFloat("CastingTime", 1f);
            if (channelling)
            {
                perception.ChangeCondition(-1);
                channelling = false;
                animator.SetBool("Casting", false);
            }
        }
        animator.SetTrigger("GetHit");
        yield return null;
        if (!perception.Locomotion.Binded)
        {
            yield return StartCoroutine(perception.Locomotion.ShovedRoutine(10));
        }
        yield return waitRecoverTime;
        getHit = false;
        if (perception.CurrentState == State.Idle)
        {
            perception.SendCommand(AlertRoutine());
        }
    }

    private IEnumerator BuffHitReactRoutine()
    {
        float duration = statusDuration;
        StartCoroutine(AdjustStatRoutine(duration, true));
        yield return null;
    }

    private IEnumerator DeBuffHitReactRoutine()
    {
        float duration = statusDuration;
        StartCoroutine(AdjustStatRoutine(duration, false));
        yield return null;
    }

    private IEnumerator MezHitReactRoutine()
    {
        animator.SetFloat("MoveSpeed", 0f);
        perception.Locomotion.Binded = true;
        float duration = statusDuration;
        yield return new WaitForSeconds(duration);
        getHit = false;
        perception.Locomotion.Binded = false;
        if (perception.CurrentState == State.Idle)
        {
            perception.SendCommand(AlertRoutine());
        }
    }

    private IEnumerator InvincibleHitReactRoutine()
    {
        float duration = statusDuration;
        StartCoroutine(InvincibleRoutine(duration));
        yield return null;
    }

    private IEnumerator RageHitReactRoutine()
    {
        foreach (Skill patern in attackPatern)
        {
            patern.energyCost /= 2;
            yield return null;
        }
        rageMode = true;
        GameManager.Sound.PlayBGM("BossBGM2");
    }

    private IEnumerator InvincibleRoutine(float duration)
    {
        invincible = true;
        yield return new WaitForSeconds(duration);
        invincible = false;
    }

    private IEnumerator AdjustStatRoutine(float duration, bool improve)
    {
        (int healthPoint, int attackPoint, float attackSpeed) originalStat = stat;
        if (improve)
        {
            stat.healthPoint *= 2;
            stat.attackPoint *= 2;
            stat.attackSpeed *= 1.2f;
        }
        else
        {
            stat.healthPoint /= 2;
            stat.attackPoint /= 2;
            stat.attackSpeed *= 0.8f;
        }
        yield return new WaitForSeconds(duration);
        stat = originalStat;
    }

    public void TakeDamaged(int damage)
    {
        int prevHp;
        int currHp;

        if (perception.CurrentCondition == Condition.Weak)
        {
            damage += damage / 2;
        }

        prevHp = stat.healthPoint;
        stat.healthPoint += -damage;
        currHp = stat.healthPoint;

        Debug.Log(stat.healthPoint);

        if (stat.healthPoint <= 0)
        {
            perception.CurrentState = State.Collapse;
        }
        else
        {
            if (getDamageSound != null && (currHp - prevHp) < 0)
            {
                GameManager.Sound.PlaySFX(getDamageSound);
            }
        }
    }

    private IEnumerator AlertRoutine()
    {
        yield return StartCoroutine(LookArountRoutine());
    }
    
    private IEnumerator LookArountRoutine()
    {
        float time = 0f;
        while (time < 1.5f)
        {
            if (perception.CurrentState != State.Idle || getHit)
            {
                yield break;
            }
            perception.SpinMonsterController(true);
            perception.Locomotion.Turn();
            perception.Vision.Gaze();
            time += Time.deltaTime;
            yield return null;
        }
        while (time < 4.5f)
        {
            if (perception.CurrentState != State.Idle || getHit)
            {
                yield break;
            }
            perception.SpinMonsterController(false);
            perception.Locomotion.Turn();
            perception.Vision.Gaze();
            time += Time.deltaTime;
            yield return null;
        }
        while (time < 6f)
        {
            if (perception.CurrentState != State.Idle || getHit)
            {
                yield break;
            }
            perception.SpinMonsterController(true);
            perception.Locomotion.Turn();
            perception.Vision.Gaze();
            time += Time.deltaTime;
            yield return null;
        }
        while (time < 7.5f)
        {
            if (perception.CurrentState != State.Idle || getHit)
            {
                yield break;
            }
            perception.Locomotion.Turn();
            perception.Vision.Gaze();
            time += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
}
