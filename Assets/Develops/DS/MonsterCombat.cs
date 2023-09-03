    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCombat : MonoBehaviour
{
    private MonsterPerception perception;

    private void Awake()
    {
        perception = GetComponent<MonsterPerception>();
    }

    private void GetDamage(Transform enemy)
    {
        perception.SpotEnemy(enemy);
    }
}
