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
    public Dictionary<MonsterTag, UnityAction> monsterAdvancedAI = new Dictionary<MonsterTag, UnityAction>();

#if UNITY_EDITOR
    private void OnEnable()
    {
        if (!monsterAdvancedAI.ContainsKey(MonsterTag.Melee))
        {
            monsterAdvancedAI.Add(MonsterTag.Melee, (() => MeleeTypeMonsterBehaviour()));
        }
        if (!monsterAdvancedAI.ContainsKey(MonsterTag.LongRange))
        {
            monsterAdvancedAI.Add(MonsterTag.LongRange, (() => LongRangeTypeMonsterBehaviour()));
        }
        if (!monsterAdvancedAI.ContainsKey(MonsterTag.Guard))
        {
            monsterAdvancedAI.Add(MonsterTag.Guard, (() => GuardTypeMonsterBehaviour()));
        }
        if (!monsterAdvancedAI.ContainsKey(MonsterTag.Aggresive))
        {
            monsterAdvancedAI.Add(MonsterTag.Aggresive, (() => AggressiveTypeMonsterBehaviour()));
        }
        if (!monsterAdvancedAI.ContainsKey(MonsterTag.SpellCaster))
        {
            monsterAdvancedAI.Add(MonsterTag.SpellCaster, (() => SpellCasterTypeMonsterBehaviour()));
        }
        if (!monsterAdvancedAI.ContainsKey(MonsterTag.Gimmick))
        {
            monsterAdvancedAI.Add(MonsterTag.Gimmick, (() => GimmickTypeMonsterBehaviour()));
        }
        if (!monsterAdvancedAI.ContainsKey(MonsterTag.Elite))
        {
            monsterAdvancedAI.Add(MonsterTag.Elite, (() => EliteTypeMonsterBehaviour()));
        }
        if (!monsterAdvancedAI.ContainsKey(MonsterTag.LastBoss))
        {
            monsterAdvancedAI.Add(MonsterTag.LastBoss, (() => LastBossTypeMonsterBehaviour()));
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
        UnityEvent advancedAI = new UnityEvent();
        foreach (MonsterTag tag in monsterInfo.monsterTag)
        {
            monsterAdvancedAI.TryGetValue(tag, out UnityAction action);
            advancedAI.AddListener(action);
            yield return null;
        }
        Vector3 guardPosition = monsterPerception.transform.position;
        while (monsterPerception.CurrentState != BasicState.Collapse)
        {
            advancedAI?.Invoke();
            yield return null;
        }
        yield return null;
    }

    private void MeleeTypeMonsterBehaviour()
    {

    }

    private void LongRangeTypeMonsterBehaviour()
    {

    }

    private void GuardTypeMonsterBehaviour()
    {
        
    }

    private void AggressiveTypeMonsterBehaviour()
    {

    }

    private void SpellCasterTypeMonsterBehaviour()
    {

    }

    private void GimmickTypeMonsterBehaviour()
    {

    }

    private void EliteTypeMonsterBehaviour()
    {

    }

    private void LastBossTypeMonsterBehaviour()
    {

    }

    [Serializable]
    public class MonsterInfo
    {
        public string monsterName;
        public GameObject monsterPrefab;
        public MonsterTag[] monsterTag;
        public IEnumerator monsterAIRoutine;
        public int healthPoint;
        public float detectRange;
        public float attackRange;
        public float moveSpeed;


    }
}
