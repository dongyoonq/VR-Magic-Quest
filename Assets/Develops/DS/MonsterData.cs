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
        monsterInfo.monsterBasicAI = TestRoutine();
    }

    //TODO: AI ���� ���� ��� ����� �� ��.
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
        public IEnumerator monsterBasicAI;
        public IEnumerator monsterAdvancedAI;
        public float attackRange;
        public float moveSpeed;


    }

    private abstract class MonsterAI : MonoBehaviour
    {
        public IEnumerator BasicMonsterBehaviourRoutine()
        {
            yield return null;
        }
        public abstract IEnumerator AdvancedMonsterBehaviourRoutine();
        // �⺻ AI(���� AI)
    }

    // ������ ��ȭ AI
    private class LichAI : MonsterAI
    {
        // ���� ����
        // �پ��� ���� ��ų
        // ���̷��� ��ȯ
        // óġ�� ���� Ŭ����
        public override IEnumerator AdvancedMonsterBehaviourRoutine()
        {
            yield return null;
        }
    }

    private class SkeletonAI : MonsterAI
    {
        // �ܼ��� ����
        public override IEnumerator AdvancedMonsterBehaviourRoutine()
        {
            yield return null;
        }
    }

    private class GolemAI : MonsterAI
    {
        // �ܼ��� ���� ���� ������
        // VR ���: ���� ����
        public override IEnumerator AdvancedMonsterBehaviourRoutine()
        {
            yield return null;
        }
    }

    private class GiantWormAI : MonsterAI
    {
        // Ư�� ���� ��Ϳ� ����
        // �ſ� ���ϰ�, ������ ��θ� ���� �̵��ϰ�
        // �÷��̾�� ������ ��� ����
        public override IEnumerator AdvancedMonsterBehaviourRoutine()
        {
            yield return null;
        }
    }
}
