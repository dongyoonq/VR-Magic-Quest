using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static EnumType;

public class MonsterCombat : MonoBehaviour, IHitReactor, IHitable
{
    private MonsterPerception perception;
    private int healthPoint;
    private int attackPoint;
    private float statusDuration;
    private Dictionary<HitTag, IEnumerator> hitReactions = new Dictionary<HitTag, IEnumerator>();

    private void Awake()
    {
        perception = GetComponent<MonsterPerception>();
        hitReactions.Add(HitTag.Impact, ImpactHitReactRoutine());
        hitReactions.Add(HitTag.Buff, BuffHitReactRoutine());
        hitReactions.Add(HitTag.Debuff, DeBuffHitReactRoutine());
        hitReactions.Add(HitTag.Mez, MezHitReactRoutine());
        hitReactions.Add(HitTag.Dot, DotHitRoutine());
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
        float duration = statusDuration;
        yield return null;
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
        yield return null;
    }

    private IEnumerator DotHitRoutine()
    {
        float duration = statusDuration;
        yield return null;
    }

    private IEnumerator AdjustStatRoutine(float duration, bool improve)
    {
        yield return null;
    }

    public void TakeDamaged(int damage)
    {
        healthPoint += -damage;
        if (healthPoint == 0)
        {
            perception.CurrentState = BasicState.Collapse;
        }
    }
}
