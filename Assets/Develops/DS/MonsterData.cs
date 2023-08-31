using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Data/Monster")]
public class MonsterData : ScriptableObject
{
    [SerializeField]
    private MonsterInfo[] monsterType;
    public MonsterInfo[] MonsterType { get { return monsterType; } }

    public void SynchronizeAI(MonsterInfo monsterInfo)
    {
        //monsterInfo.monsterAI = 
    }

    [Serializable]
    public class MonsterInfo
    {
        public string monsterName;
        public IEnumerator monsterBasicAI;
        public IEnumerator monsterAdvancedAI;
        public float attackRange;
        public float moveSpeed;


    }

    private class MonsterAI : MonoBehaviour
    {
        public IEnumerator BasicMonsterBehaviourRoutine()
        {
            yield return null;
        }
        // 기본 AI(공용 AI)
    }

    // 밑으로 심화 AI
    private class LichAI : MonsterAI
    {
        // 최종 보스
        // 다양한 마법 스킬
        // 스켈레톤 소환
        // 처치시 게임 클리어
    }

    private class SkeletonAI : MonsterAI
    {
        // 단순한 패턴
    }

    private class GolemAI : MonsterAI
    {
        // 단순한 패턴 느린 움직임
        // VR 기믹: 약점 공략
    }

    private class GiantWormAI : MonsterAI
    {
        // 특수 몬스터 기믹용 몬스터
        // 매우 강하게, 정해진 경로를 따라 이동하게
        // 플레이어와 닿으면 즉사 판정
    }
}
