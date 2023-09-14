using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumType;

public class GimmickTrigger : MonoBehaviour
{
    [SerializeField]
    int gimmickDamage;
    [SerializeField]
    HitTag[] gimmickDamageType;
    private MonsterCombat monster;

    private void Awake()
    {
        monster = GetComponentInParent<MonsterCombat>();
    }

    private void OnTriggerEnter(Collider other)
    {
        monster.TakeDamaged(gimmickDamage);
        monster.HitReact(gimmickDamageType, 1f);
        gameObject.SetActive(false);
    }
}
