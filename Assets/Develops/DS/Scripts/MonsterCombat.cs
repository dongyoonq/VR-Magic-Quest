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
    private Dictionary<HitTag, IEnumerator> hitReactions = new Dictionary<HitTag, IEnumerator>();
    private LayerMask hitTargetLayerMask;
    private Coroutine attackRoutine;
    private float attackDelayTime;
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
        hitReactions.Add(HitTag.Impact, ImpactHitReactRoutine());
        hitReactions.Add(HitTag.Buff, BuffHitReactRoutine());
        hitReactions.Add(HitTag.Debuff, DeBuffHitReactRoutine());
        hitReactions.Add(HitTag.Mez, MezHitReactRoutine());
        hitTargetLayerMask = LayerMask.GetMask("Player");
    }

    public void Combat()
    {
        if (Mathf.Abs(attackDelayTime- 0f) < 0.01f)
        {
            attackRoutine = StartCoroutine(AttackRoutine());
            attackDelayTime = -1f;
        }
        attackDelayTime -= Time.deltaTime * stat.attackSpeed;
    }

    private IEnumerator AttackRoutine()
    {
        animator.SetTrigger("Attack");
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsTag("AttackFinish"));
        Collider[] hitObjects = Physics.OverlapSphere(Camera.main.transform.position, 1f, hitTargetLayerMask);
        foreach (Collider hitObject in hitObjects)
        {
            hitObject.GetComponent<IHittable>()?.TakeDamaged(stat.attackPoint);
            hitObject.GetComponent<IHitReactor>()?.HitReact(basicAttackType, 1f);
        }
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).IsTag("AttackFinish"));
        attackDelayTime = 3f;
    }

    // TODO: 애니메이터 태그 수정 필요할 듯
    private IEnumerator MeleeAttackRoutine()
    {
        animator.SetTrigger("MeleeAttack");
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsTag("AttackFinish"));
        Collider[] hitObjects;
        foreach (Transform attackPoint in meleeAttackPoints)
        {
            hitObjects = Physics.OverlapSphere(attackPoint.position, 1f, hitTargetLayerMask);
            foreach(Collider hitObject in hitObjects)
            {
                hitObject.GetComponent<IHittable>()?.TakeDamaged(stat.attackPoint);
                hitObject.GetComponent<IHitReactor>()?.HitReact(basicAttackType, 1f);
            }
        }
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).IsTag("AttackFinish"));
    }

    private IEnumerator RangedAttackRoutine()
    {
        yield return null;
    }

    public void HitReact(HitTag[] hitType, float duration)
    {
        foreach (HitTag hitTag in hitType)
        {
            hitReactions.TryGetValue(hitTag, out IEnumerator hitReactRoutine);
            statusDuration = duration;
            perception.SendCommand(hitReactRoutine);
        }
    }

    private IEnumerator ImpactHitReactRoutine()
    {
        StopCoroutine(attackRoutine);
        //if (attackRoutine != null)
        //{
        //    StopCoroutine(attackRoutine);
        //}
        animator.SetTrigger("GetHit");
        yield return waitRecoverTime;
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
        float duration = statusDuration;
        yield return new WaitForSeconds(duration);
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
        stat.healthPoint += -damage;
        if (stat.healthPoint <= 0)
        {
            perception.CurrentState = BasicState.Collapse;
        }
    }

    private void LookAround()
    {
        // 주변 회전
    }
}
