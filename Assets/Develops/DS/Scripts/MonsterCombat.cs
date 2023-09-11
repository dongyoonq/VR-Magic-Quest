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
    private (int healthPoint, int attackPoint) stat;
    public (int healthPoint, int attackPoint) Stat { get { return stat; } set { stat = value; } }
    private WaitForSeconds waitRecoverTime;
    public WaitForSeconds WaitRecoverTime { get { return waitRecoverTime; } set { waitRecoverTime = value; } }
    private float statusDuration;
    private Dictionary<HitTag, IEnumerator> hitReactions = new Dictionary<HitTag, IEnumerator>();

    private void Awake()
    {
        perception = GetComponent<MonsterPerception>();
        animator = GetComponent<Animator>();
        hitReactions.Add(HitTag.Impact, ImpactHitReactRoutine());
        hitReactions.Add(HitTag.Buff, BuffHitReactRoutine());
        hitReactions.Add(HitTag.Debuff, DeBuffHitReactRoutine());
        hitReactions.Add(HitTag.Mez, MezHitReactRoutine());
    }

    public void Combat()
    {

    }

    private void GetDamage(Transform enemy)
    {
        perception.SpotEnemy(enemy);
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
        (int healthPoint, int attackPoint) originalStat = stat;
        if (improve)
        {
            stat.healthPoint = stat.healthPoint *= 2;
            stat.attackPoint = stat.attackPoint *= 2;
        }
        else
        {
            stat.healthPoint = stat.healthPoint / 2;
            stat.attackPoint = stat.attackPoint / 2;
        }
        yield return new WaitForSeconds(duration);
    }

    public void TakeDamaged(int damage)
    {
        stat.healthPoint += -damage;
        if (stat.healthPoint == 0)
        {
            perception.CurrentState = BasicState.Collapse;
        }
    }
}
