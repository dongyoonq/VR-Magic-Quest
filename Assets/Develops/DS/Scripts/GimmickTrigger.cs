using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumType;

public class GimmickTrigger : MonoBehaviour
{
    [SerializeField]
    private HitTag[] gimmickDamageType;
    public int gimmickDamage;
    public int gimmickHealAmount;
    private MonsterCombat monster;
    //TODO: 트리거와 기믹몬스터 사이에 연결된 선 만들기
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
