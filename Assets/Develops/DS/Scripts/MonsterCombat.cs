using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static EnumType;

public class MonsterCombat : MonoBehaviour, IHitReactor, IHittable
{
    private MonsterPerception perception;
    private Animator animator;
    [SerializeField]
    private (int healthPoint, int attackPoint, float attackSpeed) stat;
    public (int healthPoint, int attackPoint, float attackSpeed) Stat { get { return stat; } set { stat = value; } }
    private WaitForSeconds waitRecoverTime;
    public WaitForSeconds WaitRecoverTime { get { return waitRecoverTime; } set { waitRecoverTime = value; } }
    private float statusDuration;
    private LayerMask hitTargetLayerMask;
    private Coroutine attackRoutine;
    private bool getHit;
    private float attackDelayTime;
    private bool meleeType;
    public bool MeleeType { get { return meleeType; } set {  meleeType = value; } }
    private bool channelling;
    [SerializeField]
    private HitTag[] basicAttackType;
    [SerializeField]
    private HitTag[] meleeAttackType;
    [SerializeField]
    private HitTag[] rangedAttackType;
    [SerializeField]
    private Transform[] meleeAttackPoints;

    private void Awake()
    {
        perception = GetComponent<MonsterPerception>();
        animator = GetComponent<Animator>();
        hitTargetLayerMask = LayerMask.GetMask("Player");
        getHit = false;
    }

    private void OnEnable()
    {
        meleeType = false;
        channelling = false;
        attackDelayTime = 0f;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    // advanced state에 따라 다른 공격
    public void Combat()
    {
        if (attackDelayTime <= 0f)
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
            StartCoroutine(CountAttackDelayTimeRoutine());
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
        yield return null;
        //animator.SetBool("MeleeAttack", true);
        //animator.SetTrigger("Attack");
        //yield return new WaitForSeconds(1f);
        //yield return StartCoroutine(perception.Locomotion.RushRoutine(15f, 3f));
        //Debug.Log(gameObject.name);
        //animator.SetBool("MeleeAttack", false);
        //if (animator.GetCurrentAnimatorStateInfo(0).IsTag("MeleeAttack"))
        //{
        //    Collider[] hitObjects;
        //    foreach (Transform attackPoint in meleeAttackPoints)
        //    {
        //        hitObjects = Physics.OverlapSphere(attackPoint.position, 1f, hitTargetLayerMask);
        //        foreach (Collider hitObject in hitObjects)
        //        {
        //            hitObject.GetComponent<IHittable>()?.TakeDamaged(stat.attackPoint * 2);
        //            hitObject.GetComponent<IHitReactor>()?.HitReact(basicAttackType, 1f);
        //        }
        //    }
        //}      
        channelling = false;
    }

    private IEnumerator RangedAttackRoutine()
    {
        yield return null;
        //animator.SetBool("RangedAttack", true);
        //animator.SetTrigger("Attack");

        //yield return null;
        //animator.SetBool("RangedAttack", false);
    }

    private IEnumerator CastSpellRoutine()
    {
        channelling = true;
        // 캐스팅 시간 딜레이 monsterSkillInfo.SpellCastingTime
        yield return null;
        channelling = false;
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
                perception.CurrentCondition--;
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
