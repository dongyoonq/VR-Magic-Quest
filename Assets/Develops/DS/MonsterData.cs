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
        // �⺻ AI(���� AI)
    }

    // ������ ��ȭ AI
    private class LichAI : MonsterAI
    {
        // ���� ����
        // �پ��� ���� ��ų
        // ���̷��� ��ȯ
        // óġ�� ���� Ŭ����
    }

    private class SkeletonAI : MonsterAI
    {
        // �ܼ��� ����
    }

    private class GolemAI : MonsterAI
    {
        // �ܼ��� ���� ���� ������
        // VR ���: ���� ����
    }

    private class GiantWormAI : MonsterAI
    {
        // Ư�� ���� ��Ϳ� ����
        // �ſ� ���ϰ�, ������ ��θ� ���� �̵��ϰ�
        // �÷��̾�� ������ ��� ����
    }
}
