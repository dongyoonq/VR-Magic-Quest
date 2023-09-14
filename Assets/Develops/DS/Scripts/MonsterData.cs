using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static EnumType;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Data/Tag")]
public class MonsterData : ScriptableObject
{
    [SerializeField]
    private MonsterInfo[] monsterType;
    public MonsterInfo[] MonsterType { get { return monsterType; } }
    public Dictionary<MonsterTag, UnityAction<MonsterPerception>> monsterAdvancedAI = new Dictionary<MonsterTag, UnityAction<MonsterPerception>>();

#if UNITY_EDITOR
    private void OnEnable()
    {
        if (!monsterAdvancedAI.ContainsKey(MonsterTag.Melee))
        {
            monsterAdvancedAI.Add(MonsterTag.Melee, (monsterPerception) => MeleeTypeMonsterBehaviour(monsterPerception));
        }
        if (!monsterAdvancedAI.ContainsKey(MonsterTag.LongRange))
        {
            monsterAdvancedAI.Add(MonsterTag.LongRange, (monsterPerception) => LongRangeTypeMonsterBehaviour(monsterPerception));
        }
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
                    Debug.LogError($"monsterAdvancedAI Dictionary�� {tag}�� ���Ե��� ����");
                }
            }
        }
#endif
        monsterInfo.monsterAIRoutine = AdvancedMonsterBehaviourRoutine(monsterInfo, monsterPerception);
    }

    private IEnumerator AdvancedMonsterBehaviourRoutine(MonsterInfo monsterInfo, MonsterPerception monsterPerception)
    {
        UnityEvent<MonsterPerception> advancedAI = new UnityEvent<MonsterPerception>();
        WaitForSeconds waitForMakeDecision = new WaitForSeconds(3f);
        foreach (MonsterTag tag in monsterInfo.monsterTag)
        {
            monsterAdvancedAI.TryGetValue(tag, out UnityAction<MonsterPerception> action);
            advancedAI.AddListener(action);
            if (tag == MonsterTag.Elite)
            {
                waitForMakeDecision = new WaitForSeconds(1.5f);
                monsterPerception.Locomotion.EliteMonster = true;
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
                    Debug.LogError("��� ���� ��Ʈ�ѿ� ��� Ʈ���Ű� �������� �ʽ��ϴ�.");
                }
                gimmickTrigger.gameObject.SetActive(true);
            }
            else if (tag == MonsterTag.SpellCaster)
            {
                monsterPerception.Locomotion.SpellCaster = true;
            }
            else if (tag == MonsterTag.Melee)
            {
                monsterPerception.Combat.MeleeType = true;
            }
            yield return null;
        }
        Vector3 guardPosition = monsterPerception.transform.position;
        while (monsterPerception.CurrentState != EnumType.State.Collapse)
        {
            advancedAI?.Invoke(monsterPerception);
            yield return waitForMakeDecision;
        }
        yield return null;
    }

    private void MeleeTypeMonsterBehaviour(MonsterPerception monsterPerception)
    {
        // ���� elite�Ͻ� �� �տ��� ����
    }

    private void LongRangeTypeMonsterBehaviour(MonsterPerception monsterPerception)
    {
        // ���Ÿ� ����
    }

    private void GuardTypeMonsterBehaviour(MonsterPerception monsterPerception)
    {
        // idle ��ȯ�� ���� ��ġ�� ����
    }

    private void TenacityTypeMonsterBehaviour(MonsterPerception monsterPerception)
    {
        // ��Ƽ�� ȸ��
    }

    private void AggressiveTypeMonsterBehaviour(MonsterPerception monsterPerception)
    {
        // idle ��ȯ�� �������� ���� �Ÿ���ŭ �̵�
    }

    private void DynamicallyMoveTypeMonsterBehaviour(MonsterPerception monsterPerception)
    {
        monsterPerception.DynamicallyMove();
    }

    private void CautiousTypeMonsterBehaviour(MonsterPerception monsterPerception)
    {
        // ������ �ڸ� ���ƺ�
    }

    private void SpellCasterTypeMonsterBehaviour(MonsterPerception monsterPerception)
    {
        // ���ǿ� ���� �پ��� ���� ��� ������ �������� �з� ����ȭ �迭�� ����
    }

    private void AgileTypeMonsterBehaviour(MonsterPerception monsterPerception)
    {
        // overap sphere �ֺ� ��ų? ���̾� ���� �� ȸ��
        if (monsterPerception.CurrentState != EnumType.State.Idle)
        {
            if (Physics.OverlapSphere(monsterPerception.transform.position, 5f, LayerMask.GetMask("Skill")).Length > 0)
            {
                
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
        //���鿡 ray�� ���� player�� �ƴѰ� ������ ���ư���
    }

    private void LastBossTypeMonsterBehaviour(MonsterPerception monsterPerception)
    {
        // 1������ 2������
        // ����� ���� Ŭ����
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
