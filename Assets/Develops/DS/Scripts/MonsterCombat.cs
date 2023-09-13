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

    private IEnumerator TestRoutine()
    {
        yield return new WaitForSeconds(3f);
        perception.SendCommand(AlertRoutine());
    }

    public void Combat()
    {
        if (Mathf.Abs(attackDelayTime- 0f) < 0.01f)
        {
            attackRoutine = StartCoroutine(BasicAttackRoutine());
            attackDelayTime = -1f;
        }
        if (attackDelayTime < 0.01f)
        {
            perception.Vision.AvertEye();
            perception.Vision.AvertEye();
        }
        attackDelayTime -= Time.deltaTime * stat.attackSpeed;
    }

    private void Attack(int damage, HitTag[] attackType)
    {
        Collider[] hitObjects = Physics.OverlapSphere(Camera.main.transform.position, 1f, hitTargetLayerMask);
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
        attackDelayTime = 3f;
    }

    public void MeleeAttack()
    {
        perception.SendCommand(MeleeAttackRoutine());
    }

    // TODO: 애니메이터 태그 수정 필요할 듯
    private IEnumerator MeleeAttackRoutine()
    {
        Debug.Log(gameObject.name);
        animator.SetBool("MeleeAttack", true);
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(perception.Locomotion.RushRoutine(15f, 3f));
        Debug.Log(gameObject.name);
        animator.SetBool("MeleeAttack", false);
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("MeleeAttack"))
        {
            Collider[] hitObjects;
            foreach (Transform attackPoint in meleeAttackPoints)
            {
                hitObjects = Physics.OverlapSphere(attackPoint.position, 1f, hitTargetLayerMask);
                foreach (Collider hitObject in hitObjects)
                {
                    hitObject.GetComponent<IHittable>()?.TakeDamaged(stat.attackPoint);
                    hitObject.GetComponent<IHitReactor>()?.HitReact(basicAttackType, 1f);
                }
            }
        }      
    }

    private IEnumerator RangedAttackRoutine()
    {
        animator.SetBool("RangedAttack", true);
        animator.SetTrigger("Attack");

        yield return null;
        animator.SetBool("RangedAttack", false);
    }

    public void HitReact(HitTag[] hitType, float duration)
    {
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
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
        }
        animator.SetTrigger("GetHit");
        yield return null;
        yield return StartCoroutine(perception.Locomotion.ShovedRoutine(10));
        yield return waitRecoverTime;
        //perception.SendCommand(AlertRoutine());
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
        //perception.SendCommand(AlertRoutine());
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
        stat.healthPoint += -damage;
        if (stat.healthPoint <= 0)
        {
            perception.CurrentState = BasicState.Collapse;
        }
    }

    private IEnumerator AlertRoutine()
    {
        Coroutine lookAroundRoutine = StartCoroutine(LookAroundRoutine());
        yield return new WaitWhile(() => perception.CurrentState == BasicState.Idle);
        StopCoroutine(lookAroundRoutine);
    }

    private IEnumerator LookAroundRoutine()
    {
        float time = 0f;
        while (time < 1.5f)
        {
            perception.SpinMonsterController(true);
            perception.Locomotion.Turn();
            perception.Vision.Gaze();
            time += Time.deltaTime;
            yield return null;
        }
        while (time < 4.5f)
        {
            perception.SpinMonsterController(false);
            perception.Locomotion.Turn();
            perception.Vision.Gaze();
            time += Time.deltaTime;
            yield return null;
        }
        while (time < 6f)
        {
            perception.SpinMonsterController(true);
            perception.Locomotion.Turn();
            perception.Vision.Gaze();
            time += Time.deltaTime;
            yield return null;
        }
        while (time < 7.5f)
        {
            perception.Locomotion.Turn();
            perception.Vision.Gaze();
            time += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
}
