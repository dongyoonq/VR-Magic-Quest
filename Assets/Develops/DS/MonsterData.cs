using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Data/Monster")]
public class MonsterData : ScriptableObject
{
    [SerializeField]
    private MonsterInfo[] monsterSpecies;
    public MonsterInfo[] MonsterSpecies { get { return monsterSpecies; } }
    protected Dictionary<string, IEnumerator> monsterType = new Dictionary<string, IEnumerator>();

    private void OnEnable()
    {
        
    }

    public void SynchronizeAI(ref MonsterInfo monsterInfo)
    {
        MonsterAI monsterAI = new MonsterAI();
        monsterInfo.monsterBasicAI = monsterAI.BasicMonsterBehaviourRoutine();
    }

    [Serializable]
    public class MonsterInfo
    {
        public string monsterName;
        public GameObject monster;
        public IEnumerator monsterBasicAI;
        public IEnumerator monsterAdvancedAI;
        public bool isEliteMonster;
        public float attackRange;
        public float moveSpeed;


    }

    public class MonsterAI : MonoBehaviour
    {
        public IEnumerator BasicMonsterBehaviourRoutine()
        {
            MonsterPerception perception = GetComponent<MonsterPerception>();
            while (perception.CurrentState != MonsterPerception.BasicState.Collapse)
            {
                switch (perception.CurrentState)
                {
                    case MonsterPerception.BasicState.Alert:
                        break;
                    case MonsterPerception.BasicState.Chase:
                        break;
                    case MonsterPerception.BasicState.Combat:
                        break;
                    case MonsterPerception.BasicState.Flee:
                        break;
                    case MonsterPerception.BasicState.Collapse:
                        break;
                    default:
                        break;
                }
                yield return null;
            }
            yield return null;
        }
    }
}
