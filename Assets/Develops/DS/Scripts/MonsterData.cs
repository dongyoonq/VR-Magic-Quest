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
        if (!monsterAdvancedAI.ContainsKey(MonsterTag.Aggresive))
        {
            monsterAdvancedAI.Add(MonsterTag.Aggresive, (monsterPerception) => AggressiveTypeMonsterBehaviour(monsterPerception));
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
        WaitForSeconds waitForMakeDecision = new WaitForSeconds(3f);
        foreach (MonsterTag tag in monsterInfo.monsterTag)
        {
            monsterAdvancedAI.TryGetValue(tag, out UnityAction<MonsterPerception> action);
            advancedAI.AddListener(action);
            if (tag == MonsterTag.Elite)
            {
                waitForMakeDecision = new WaitForSeconds(1.5f);
            }
            yield return null;
        }
        Vector3 guardPosition = monsterPerception.transform.position;
        while (monsterPerception.CurrentState != BasicState.Collapse)
        {
            advancedAI?.Invoke(monsterPerception);
            yield return waitForMakeDecision;
        }
        yield return null;
    }

    private void MeleeTypeMonsterBehaviour(MonsterPerception monsterPerception)
    {

    }

    private void LongRangeTypeMonsterBehaviour(MonsterPerception monsterPerception)
    {

    }

    private void GuardTypeMonsterBehaviour(MonsterPerception monsterPerception)
    {
        
    }

    private void AggressiveTypeMonsterBehaviour(MonsterPerception monsterPerception)
    {

    }

    private void SpellCasterTypeMonsterBehaviour(MonsterPerception monsterPerception)
    {

    }

    private void AgileTypeMonsterBehaviour(MonsterPerception monsterPerception)
    {
        // overap sphere 주변 스킬? 레이어 감지 시 회피
    }

    private void GimmickTypeMonsterBehaviour(MonsterPerception monsterPerception)
    {

    }

    private void EliteTypeMonsterBehaviour(MonsterPerception monsterPerception)
    {

    }

    private void LastBossTypeMonsterBehaviour(MonsterPerception monsterPerception)
    {
        // 1페이즈 2페이즈
        // 사망시 게임 클리어
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
        public float moveSpeed;


    }
}
