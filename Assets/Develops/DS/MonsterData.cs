using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Data/Tag")]
public class MonsterData : ScriptableObject
{
    [SerializeField]
    private MonsterInfo[] monsterType;
    public MonsterInfo[] MonsterType { get { return monsterType; } }
    public Dictionary<Tag, UnityAction> monsterAdvancedAI = new Dictionary<Tag, UnityAction>();

    public enum Tag
    {
        Melee,
        LongRange,
        Defensive,
        Aggresive,
        SpellCaster,
        Gimmick,
        Elite,
        LastBoss
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        if (!monsterAdvancedAI.ContainsKey(Tag.Melee))
        {
            monsterAdvancedAI.Add(Tag.Melee, (() => MeleeTypeMonsterBehaviour()));
        }
        if (!monsterAdvancedAI.ContainsKey(Tag.LongRange))
        {
            monsterAdvancedAI.Add(Tag.LongRange, (() => LongRangeTypeMonsterBehaviour()));
        }
        if (!monsterAdvancedAI.ContainsKey(Tag.Defensive))
        {
            monsterAdvancedAI.Add(Tag.Defensive, (() => DefensiveTypeMonsterBehaviour()));
        }
        if (!monsterAdvancedAI.ContainsKey(Tag.Aggresive))
        {
            monsterAdvancedAI.Add(Tag.Aggresive, (() => AggressiveTypeMonsterBehaviour()));
        }
        if (!monsterAdvancedAI.ContainsKey(Tag.SpellCaster))
        {
            monsterAdvancedAI.Add(Tag.SpellCaster, (() => SpellCasterTypeMonsterBehaviour()));
        }
        if (!monsterAdvancedAI.ContainsKey(Tag.Gimmick))
        {
            monsterAdvancedAI.Add(Tag.Gimmick, (() => GimmickTypeMonsterBehaviour()));
        }
        if (!monsterAdvancedAI.ContainsKey(Tag.Elite))
        {
            monsterAdvancedAI.Add(Tag.Elite, (() => EliteTypeMonsterBehaviour()));
        }
        if (!monsterAdvancedAI.ContainsKey(Tag.LastBoss))
        {
            monsterAdvancedAI.Add(Tag.LastBoss, (() => LastBossTypeMonsterBehaviour()));
        }
    }
#endif

    public void SynchronizeAI(ref MonsterInfo monsterInfo, MonsterPerception monsterPerception)
    {
#if UNITY_EDITOR
        if (monsterAdvancedAI.Count != Enum.GetValues(typeof(Tag)).Length)
        {
            foreach (Tag tag in Enum.GetValues(typeof(Tag)))
            {
                if (!monsterAdvancedAI.ContainsKey(tag))
                {
                    Debug.LogError($"monsterAdvancedAI Dictionary에 {tag}가 포함되지 않음");
                }
            }
        }
#endif
        monsterInfo.monsterBasicAI = BasicMonsterBehaviourRoutine(monsterPerception);
        monsterInfo.monsterAdvancedAI = AdvancedMonsterBehaviourRoutine(monsterInfo, monsterPerception);
    }

    private IEnumerator BasicMonsterBehaviourRoutine(MonsterPerception monsterPerception)
    {
        yield return null;
        //MonsterPerception perception = GetComponent<MonsterPerception>();
        //while (perception.CurrentState != MonsterPerception.BasicState.Collapse)
        //{
        //    switch (perception.CurrentState)
        //    {
        //        case MonsterPerception.BasicState.Alert:
        //            break;
        //        case MonsterPerception.BasicState.Chase:
        //            break;
        //        case MonsterPerception.BasicState.Combat:
        //            break;
        //        case MonsterPerception.BasicState.Flee:
        //            break;
        //        case MonsterPerception.BasicState.Collapse:
        //            break;
        //        default:
        //            break;
        //    }
        //    yield return null;
        //    perception.Test();
        //}
        //yield return null;
    }

    private IEnumerator AdvancedMonsterBehaviourRoutine(MonsterInfo monsterInfo, MonsterPerception monsterPerception)
    {
        UnityEvent advancedAI = new UnityEvent();
        foreach (Tag tag in monsterInfo.monsterTag)
        {
            monsterAdvancedAI.TryGetValue(tag, out UnityAction action);
            advancedAI.AddListener(action);
            yield return null;
        }
        while (monsterPerception.CurrentState != MonsterPerception.BasicState.Collapse)
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

    private void DefensiveTypeMonsterBehaviour()
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
        public Tag[] monsterTag;
        public IEnumerator monsterBasicAI;
        public IEnumerator monsterAdvancedAI;
        public bool isEliteMonster;
        public float attackRange;
        public float moveSpeed;


    }
}
