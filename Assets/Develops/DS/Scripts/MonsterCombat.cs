using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using static EnumType;
using static MonsterCombat;

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
    public bool rageMode;
    [SerializeField]
    private MonsterSkillData skillData;
    [SerializeField]
    private GameObject concentrateObject;
    private float attackDelayTime;
    [SerializeField]
    private Skill heavyAttack;
    public Skill HeavyAttack {  get { return heavyAttack; } set {  heavyAttack = value; } }
    [SerializeField]
    private Skill[] harassmentSkills;
    public Skill[] HarassmentSkills { get {  return harassmentSkills; } set {  harassmentSkills = value; } }
    [SerializeField]
    private Skill[] aggressiveSkills;
    public Skill[] AggressiveSkills { get { return aggressiveSkills; } set {  aggressiveSkills = value; } }
    [SerializeField]
    private Skill[] defensiveSkills;
    public Skill[] DefensiveSkills { get { return defensiveSkills; } set {  defensiveSkills = value; } }
    [SerializeField]
    private Skill[] finisher;
    public Skill[] Finisher { get { return finisher; } set { finisher = value; } }
    private bool channelling;
    [SerializeField]
    private HitTag[] basicAttackType;
    [SerializeField]
    private HitTag[] heavyAttackType;

    [Serializable]
    public class Skill
    {
        public MonsterSkill skill;
        public int energyCost;
        public int limit;
    }

    private void Awake()
    {
        perception = GetComponent<MonsterPerception>();
        animator = GetComponent<Animator>();
        hitTargetLayerMask = LayerMask.GetMask("Player");
        getHit = false;
        concentrateObject = GameManager.Resource.Instantiate(concentrateObject, transform.position + Vector3.up * 2f, transform.rotation);
        concentrateObject.transform.SetParent(transform);
        concentrateObject.SetActive(false);
    }

    private void OnEnable()
    {
        channelling = false;
        rageMode = false;
        attackDelayTime = 0f;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    //TODO: 공격 딜레이 타임 조정, 돌진, 회피
    // advanced state에 따라 다른 공격
    public void Combat()
    {
        Debug.Log(attackDelayTime);
        if (attackDelayTime <= 0f)
        {
            if (heavyAttack.limit == 0)
            {
                attackRoutine = StartCoroutine(BasicAttackRoutine());
            }
            else
            {
                if (perception.CurrentCondition <= Condition.Good)
                {
                    attackRoutine = StartCoroutine(BasicAttackRoutine());
                }
                else if (perception.CurrentCondition > Condition.Good)
                {
                    attackRoutine = StartCoroutine(HeavytAttackRoutine());
                    perception.CurrentCondition--;
                }
            }
            attackDelayTime = 100f;
            StartCoroutine(CountAttackDelayTimeRoutine());
        }
        else
        {
            if (perception.CurrentCondition >= Condition.Energetic)
            {
                CheckConditionAndUse(harassmentSkills);
                CheckConditionAndUse(finisher);
            }
            else if (stat.healthPoint <= 200)
            {
                CheckConditionAndUse(DefensiveSkills);
            }
            else if (perception.CurrentCondition >= Condition.Tired && perception.
                CompareDistanceWithoutHeight(transform.position, perception.Vision.TargetTransform.position, 20f))
            {
                CheckConditionAndUse(aggressiveSkills);
            }
        }
    }

    private IEnumerator CountAttackDelayTimeRoutine()
    {
        while (attackDelayTime <= 0f)
        {
            if (perception.CurrentCondition <= Condition.Exhausted)
            {
                attackDelayTime -= Time.deltaTime * stat.attackSpeed;
            }
            else if (perception.CurrentCondition > Condition.Energetic)
            {
                attackDelayTime -= Time.deltaTime * stat.attackSpeed;
                attackDelayTime -= Time.deltaTime * stat.attackSpeed;
                attackDelayTime -= Time.deltaTime * stat.attackSpeed;
            }
            else
            {
                attackDelayTime -= Time.deltaTime * stat.attackSpeed;
                attackDelayTime -= Time.deltaTime * stat.attackSpeed;
            }
            if (attackDelayTime < 0.01f)
            {
                perception.Vision.AvertEye();
                perception.Vision.AvertEye();
            }
            yield return null;
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

    private IEnumerator HeavytAttackRoutine()
    {
        channelling = true;
        heavyAttack.limit--;
        yield return null;
        if (heavyAttack.skill == MonsterSkill.Basic)
        {
            animator.SetTrigger("Attack");
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsTag("AttackFinish"));
            Attack(stat.attackPoint * 2, basicAttackType);
            yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).IsTag("AttackFinish"));
        }
        else
        {
            Cast(ref heavyAttack);
        }  
        channelling = false;
    }

    public void Cast(ref Skill skill)
    {
        perception.ChangeCondition(-skill.energyCost);
        skill.limit--;
        concentrateRoutine = StartCoroutine(CastSpellRoutine(skillData.GetSkillInfo(skill.skill)));
        perception.SendCommand(ConcentrateRoutine());
    }

    private IEnumerator CastSpellRoutine(MonsterSkillData.SkillInfo skillInfo)
    {
        channelling = true;
        yield return new WaitForSeconds(skillInfo.castingTime);
        ActivateSpell(skillInfo, perception.Vision.TargetTransform);
        channelling = false;
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
                break;
            case Aim.Front:
                spell = GameManager.Resource.Instantiate
                    (skillPrefab, transform.position + transform.forward, transform.rotation, true).GetComponent<Spell>();
                break;
            case Aim.Around:
                Vector3 aroundPosition = UnityEngine.Random.insideUnitSphere * 2f + targetTransform.position;
                spell = GameManager.Resource.Instantiate
                    (skillPrefab, new Vector3(aroundPosition.x, targetTransform.position.y, aroundPosition.z), targetTransform.rotation, true)
                   .GetComponent<Spell>();
                break;
            default:
                spell = GameManager.Resource.Instantiate(skillPrefab, transform.position, transform.rotation, true)
                    .GetComponent<Spell>();
                break;
        }
        spell.PreviousSpell = targetTransform.GetComponent<Spell>();
        spell.SynchronizeSpell(skillInfo, transform);
        if (skillInfo.additionalSkills.Length > 0)
        {
            for(int i = 0; i < skillInfo.additionalSkills.Length; i++)
            {
                ActivateSpell(skillInfo.additionalSkills[i], spell.transform);
            }
        }
    }

    private IEnumerator ConcentrateRoutine()
    {
        animator.SetBool("Casting", true);
        concentrateObject.SetActive(true);
        yield return new WaitWhile(() => channelling);
        concentrateObject.SetActive(false);
    }

    public void Meditation()
    {
        channelling = true;
        concentrateRoutine = StartCoroutine(MeditationRoutine());
        perception.SendCommand(ConcentrateRoutine());
    }

    private IEnumerator MeditationRoutine()
    {
        channelling = false;
        int time = 0;
        while(perception.CurrentCondition <= Condition.TopForm || time == 5)
        {
            yield return new WaitForSeconds(1f);
            time++;
            perception.ChangeCondition(2);
        }
    }

    public void HitReact(HitTag[] hitType, float duration)
    {
        getHit = true;
        foreach (HitTag hitTag in hitType)
        {
            statusDuration = duration;
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
                channelling = false;
            }            
        }
        if (concentrateRoutine != null)
        {
            StopCoroutine(concentrateRoutine);
            if (channelling)
            {
                perception.ChangeCondition(-1);
                channelling = false;
            }
        }
        animator.SetTrigger("GetHit");
        yield return null;
        yield return StartCoroutine(perception.Locomotion.ShovedRoutine(10));
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
        float duration = statusDuration;
        yield return new WaitForSeconds(duration);
        getHit = false;
        if (perception.CurrentState == State.Idle)
        {
            perception.SendCommand(AlertRoutine());
        }
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
        // TODO: 강한 피해를 받으면 혹은 데미지 정도에 따라 밀려나는 거리를 다르게 하고 싶으면 perception.SendCommand(perception.Locomotion.ShovedRoutine(10));
        if (perception.CurrentCondition == Condition.Weak)
        {
            damage += damage / 2;
        }
        stat.healthPoint += -damage;
        if (stat.healthPoint <= 0)
        {
            perception.CurrentState = State.Collapse;
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
