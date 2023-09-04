    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumType;

public class MonsterCombat : MonoBehaviour, IHitReactor
{
    private MonsterPerception perception;

    private void Awake()
    {
        perception = GetComponent<MonsterPerception>();
    }

    public void Combat()
    {

    }

    private void GetDamage(Transform enemy)
    {
        perception.SpotEnemy(enemy);
    }

    public void HitReact(HitTag[] hitType, int damage)
    {
        foreach (HitTag hitTag in hitType)
        {

        }
    }
}
