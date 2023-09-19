using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static EnumType;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Data/MonsterData")]
public class MonsterData : ScriptableObject
{
    [SerializeField]
    private MonsterInfo[] monsterType;
    public MonsterInfo[] MonsterType { get { return monsterType; } }
    public Dictionary<MonsterTag, UnityAction<MonsterPerception>> monsterAdvancedAI = new Dictionary<MonsterTag, UnityAction<MonsterPerception>>();

#if UNITY_EDITOR
    private void OnEnable()
    {
        if (!monsterAdvancedAI.ContainsKey(MonsterTag.Guard))
        {
            monsterAdvancedAI.Add(MonsterTag.Guard, (monsterPerception) => GuardTypeMonsterBehaviour(monsterPerception));
        }
        if (!monsterAdvancedAI.ContainsKey(MonsterTag.Tenacity))
        {
            monsterAdvancedAI.Add(MonsterTag.Tenacity, (monsterPerception) => TenacityTypeMonsterBehaviour(monsterPerception));
        }
        if (!monsterAdvancedAI.ContainsKey(MonsterTag.Aggresive))
        {
            monsterAdvancedAI.Add(MonsterTag.Aggresive, (monsterPerception) => AggressiveTypeMonsterBehaviour(monsterPerception));
        }
        if (!monsterAdvancedAI.ContainsKey(MonsterTag.DynamicallyMove))
        {
            monsterAdvancedAI.Add(MonsterTag.DynamicallyMove, (monsterPerception) => DynamicallyMoveTypeMonsterBehaviour(monsterPerception));
        }
        if (!monsterAdvancedAI.ContainsKey(MonsterTag.Cautious))
        {
            monsterAdvancedAI.Add(MonsterTag.Cautious, (monsterPerception) => CautiousTypeMonsterBehaviour(monsterPerception));
        }
        if (!monsterAdvancedAI.ContainsKey(MonsterTag.SpellCaster))
        {
            monsterAdvancedAI.Add(MonsterTag.SpellCaster, (monsterPerception) => SpellCasterTypeMonsterBehaviour(monsterPerception));
        }
        if (!monsterAdvancedAI.ContainsKey(MonsterTag.Agile))
        {
            monsterAdvancedAI.Add(MonsterTag.Agile, (monsterPerception) => AgileTypeMonsterBehaviour(monsterPerception));
        }
        if (!monsterAdvancedAI.ContainsKey(MonsterTag.Gimmick))
        {
            monsterAdvancedAI.Add(MonsterTag.Gimmick, (monsterPerception) => GimmickTypeMonsterBehaviour(monsterPerception));
        }
        if (!monsterAdvancedAI.ContainsKey(MonsterTag.Elite))
        {
            monsterAdvancedAI.Add(MonsterTag.Elite, (monsterPerception) => EliteTypeMonsterBehaviour(monsterPerception));
        }
        if (!monsterAdvancedAI.ContainsKey(MonsterTag.LastBoss))
        {
            monsterAdvancedAI.Add(MonsterTag.LastBoss, (monsterPerception) => LastBossTypeMonsterBehaviour(monsterPerception));
        }
    }
#endif

    public void SynchronizeAI(ref MonsterInfo monsterInfo, MonsterPerception monsterPerception)
    {
#if UNITY_EDITOR
        if (monsterAdvancedAI.Count != Enum.GetValues(typeof(MonsterTag)).Length)
        {
            foreach (MonsterTag tag in Enum.GetValues(typeof(MonsterTag)))
            {
                if (!monsterAdvancedAI.ContainsKey(tag))
                {
                    Debug.LogError($"monsterAdvancedAI Dictionary에 {tag}가 포함되지 않음");
                }
            }
        }
#endif
        monsterInfo.monsterAIRoutine = AdvancedMonsterBehaviourRoutine(monsterInfo, monsterPerception);
    }

    private IEnumerator AdvancedMonsterBehaviourRoutine(MonsterInfo monsterInfo, MonsterPerception monsterPerception)
    {
        UnityEvent<MonsterPerception> advancedAI = new UnityEvent<MonsterPerception>();
        WaitForSeconds waitForMakeDecision = new WaitForSeconds(10f);
        foreach (MonsterTag tag in monsterInfo.monsterTag)
        {
            monsterAdvancedAI.TryGetValue(tag, out UnityAction<MonsterPerception> action);
            advancedAI.AddListener(action);
            if (tag == MonsterTag.Elite)
            {
                waitForMakeDecision = new WaitForSeconds(5f);
                monsterPerception.Locomotion.EliteMonster = true;
            }
            else if (tag == MonsterTag.Guard)
            {
                monsterPerception.Locomotion.GuardPosition = monsterPerception.transform.position;
            }
            else if (tag == MonsterTag.Cautious)
            {
                monsterPerception.alertMoveSpeed *= 0.5f;
            }
            else if (tag == MonsterTag.Tenacity)
            {
                monsterPerception.AdjustRecoverTime(0.25f);
            }
            else if (tag == MonsterTag.Gimmick)
            {
                GimmickTrigger gimmickTrigger = monsterPerception.GimmickTrigger;
                if (gimmickTrigger == null)
                {
                    Debug.LogError("기믹 몬스터 컨트롤에 기믹 트리거가 존재하지 않습니다.");
                }
                gimmickTrigger.gameObject.SetActive(true);
            }
            else if (tag == MonsterTag.SpellCaster)
            {
                monsterPerception.Locomotion.SpellCaster = true;
            }
            else if (tag == MonsterTag.Agile)
            {
                monsterPerception.Locomotion.DodgeEffect = GameManager.Resource.Instantiate(monsterPerception.Locomotion.DodgeEffect);
            }
            yield return null;
        }
        while (monsterPerception.CurrentState != EnumType.State.Collapse)
        {
            advancedAI?.Invoke(monsterPerception);
            yield return waitForMakeDecision;
        }
        yield return null;
    }

    private void GuardTypeMonsterBehaviour(MonsterPerception monsterPerception)
    {
        if (monsterPerception.CurrentState == EnumType.State.Idle && !monsterPerception.Locomotion.Moving &&
            monsterPerception.CompareDistanceWithoutHeight(monsterPerception.transform.position, monsterPerception.Locomotion.GuardPosition, 1f))
        {
            monsterPerception.StartCoroutine(monsterPerception.ReturnRoutine());
        }
    }

    private void TenacityTypeMonsterBehaviour(MonsterPerception monsterPerception)
    {
        monsterPerception.Combat.TakeDamaged(-5);
    }

    private void AggressiveTypeMonsterBehaviour(MonsterPerception monsterPerception)
    {
        //monsterPerception.StartCoroutine(monsterPerception.Locomotion.RushRoutine());
        //MonsterCombat.Skill[] heavyAttack = { monsterPerception.Combat.HeavyAttack };
        //monsterPerception.Combat.CheckConditionAndUse(heavyAttack);
    }

    private void DynamicallyMoveTypeMonsterBehaviour(MonsterPerception monsterPerception)
    {
        monsterPerception.DynamicallyMove();
    }

    private void CautiousTypeMonsterBehaviour(MonsterPerception monsterPerception)
    {
        if (monsterPerception.CurrentState == EnumType.State.Idle)
        {
            monsterPerception.Vision.CheckBehind();
        }       
    }

    private void SpellCasterTypeMonsterBehaviour(MonsterPerception monsterPerception)
    {
        if (monsterPerception.CurrentState == EnumType.State.Combat)
        {
            monsterPerception.Locomotion.StartCoroutine
                (monsterPerception.Locomotion.KeepDistanceRoutine
                (monsterPerception.alertMoveSpeed, monsterPerception.CompareDistanceWithoutHeight
                (monsterPerception.transform.position, monsterPerception.Vision.TargetTransform.position, 7f)));
        }
    }

    private void AgileTypeMonsterBehaviour(MonsterPerception monsterPerception)
    {
        if (monsterPerception.CurrentState != EnumType.State.Idle)
        {
            Collider[] collider = Physics.OverlapSphere(monsterPerception.transform.position, 5f, LayerMask.GetMask("Skill"));
            if (collider.Length > 0)
            {
                monsterPerception.Locomotion.Dodge(collider[0].transform.position);
            }
            else
            {
                if (!monsterPerception.CompareDistanceWithoutHeight(monsterPerception.transform.position, monsterPerception.Vision.TargetTransform.position, 5f))
                {
                    monsterPerception.SendCommand(monsterPerception.Locomotion.TeleportRoutine());
                }
            }
        }       
    }

    private void GimmickTypeMonsterBehaviour(MonsterPerception monsterPerception)
    {
        if(monsterPerception.GimmickTrigger.gameObject.activeSelf)
        {
            monsterPerception.Combat.TakeDamaged(-monsterPerception.GimmickTrigger.gimmickHealAmount);
        }
    }

    private void EliteTypeMonsterBehaviour(MonsterPerception monsterPerception)
    {
        monsterPerception.ChangeCondition(1);
    }

    private void LastBossTypeMonsterBehaviour(MonsterPerception monsterPerception)
    {
        if (monsterPerception.Combat.rageMode)
        {

        }
    }

    [Serializable]
    public class MonsterInfo
    {
        public string monsterName;
        public GameObject monsterPrefab;
        public MonsterTag[] monsterTag;
        public GameObject[] dropItems;
        public IEnumerator monsterAIRoutine;
        public int healthPoint;
        public int attackPoint;
        public float detectRange;
        public float attackRange;
        public float attackSpeed;
        public float moveSpeed;
    }
}
