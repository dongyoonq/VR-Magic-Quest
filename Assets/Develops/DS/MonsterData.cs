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

    public void SynchronizeAI(MonsterInfo monsterInfo)
    {
        //monsterInfo.monsterBasicAI = 
    }

    //TODO: AI 종류 저장 방식은 나중에 더 고민해 볼 것.
    private IEnumerator TestRoutine()
    {
        yield return null;
        Debug.Log("TestSuccess");
    }

    [Serializable]
    public class MonsterInfo
    {
        public string monsterName;
        public GameObject monster;
        public IEnumerator monsterAdvancedAI;
        public bool isEliteMonster;
        public float attackRange;
        public float moveSpeed;

        public IEnumerator monsterBasicAI()
        {
            yield return null;
        }
    }

    
}
