using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static EnumType;

public class MonsterPerception : MonoBehaviour
{
    private MonsterController controller;
    private MonsterData.MonsterInfo monsterInfo;
    private MonsterVision vision;
    private MonsterCombat combat;
    private MonsterLocomotion locomotion;
    private BasicState currentState;
    public BasicState CurrentState { get { return currentState; } set { currentState = value; } }
    private IEnumerator advancedAI;
    public float alertMoveSpeed;
    public float chaseMoveSpeed;

    private void Awake()
    {
        vision = GetComponent<MonsterVision>();
        combat = GetComponent<MonsterCombat>();
        locomotion = GetComponent<MonsterLocomotion>();
    }

    public IEnumerator MakeDecisionRoutine()
    {
        advancedAI = monsterInfo.monsterAIRoutine;
        Coroutine advancedAIRoutine = StartCoroutine(advancedAI);
        yield return new WaitUntil(() => currentState == BasicState.Collapse);
        StopCoroutine(advancedAIRoutine);
        yield return StartCoroutine(CollapseRoutine());
        GameManager.Resource.Destroy(gameObject);
    }

    public void SpotEnemy(Transform enemyTransform)
    {
        currentState = BasicState.Alert;
        controller.transform.position = enemyTransform.position;
        controller.transform.parent = enemyTransform;
    }

    // guard Ÿ���� ��ġ�� ���� �ڸ��� ���� aggressive�� ���� ���ĵ� �ش� �������� ���� �Ÿ���ŭ �� ����
    public void LoseSightOfTarget()
    {
        currentState = BasicState.Idle;
    }

    public void SendCommand(IEnumerator command)
    {
        controller.GetCommand(command);
    }

    private void MonsterBasicBehave()
    {
        switch (currentState)
        {
            case BasicState.Idle:
                break;
            case BasicState.Alert:
                locomotion.Approach(alertMoveSpeed);
                vision.Gaze();
                break;
            case BasicState.Chase:
                locomotion.Approach(chaseMoveSpeed);
                vision.Gaze();
                break;
            case BasicState.Combat:
                vision.Gaze();
                combat.Combat();
                break;
            default:
                break;
        }
        // ���� ��Ÿ� ���ϸ� ����, current state�� idle�̰� �÷��̾ �ڸ� ���̰� ������ chase �ƴϸ� alert detectrange���� �־����� idle
        if (CompareDistanceWithoutHeight(controller.transform.position, transform.position, monsterInfo.attackRange))
        {
            if(currentState == BasicState.Idle)
            {
                return;
            }
            if (CheckBackAttackChance())
            {
                currentState = BasicState.Chase;
            }
            else
            {
                currentState = BasicState.Alert;
            }
        }
        else
        {
            currentState = BasicState.Combat;
        }

    }

    public IEnumerator MoveRoutine()
    {
        yield return null;
    }

    private IEnumerator CollapseRoutine()
    {
        StopCoroutine(controller.monsterBehaviourRoutine);
        // �״� �ִϸ��̼�
        // vr ��ȣ�ۿ� Ȱ��ȭ
        // �ӽ� ������ ��� ���߿� ��ȣ�ۿ�� ���������� ���ϴ� ������ ������ ��
        if(monsterInfo.dropItems.Length > 0)
        {
            GameManager.Resource.Instantiate(monsterInfo.dropItems[Random.Range(0, monsterInfo.dropItems.Length)], true);
        }
        // ��ȣ�ۿ�� ���������� ����(�������� ����Ʈ���� pool ȸ��)
        yield return null;
    }

    public void ActivateMonster(MonsterController monsterController, MonsterData.MonsterInfo monsterInfo)
    {
        this.controller = monsterController;
        this.monsterInfo = monsterInfo;
        alertMoveSpeed = monsterInfo.moveSpeed;
        chaseMoveSpeed = monsterInfo.moveSpeed;
        currentState = BasicState.Idle;
        SynchronizeController((() => MonsterBasicBehave()), true);
        SynchronizeVision();
        SynchronizeCombat();
        advancedAI = monsterInfo.monsterAIRoutine;
        StartCoroutine(MakeDecisionRoutine());
    }

    public void SynchronizeController(UnityAction action, bool active)
    {
        if (active)
        {
            controller.activeEvent.AddListener(action);
        }
        else
        {
            controller.passiveEvent.AddListener(action);
        }
    }

    public void SynchronizeVision()
    {
        vision.DetectRange.radius = monsterInfo.detectRange;
    }

    public void SynchronizeCombat()
    {
        combat.Stat = (monsterInfo.healthPoint, monsterInfo.attackPoint);
        combat.WaitRecoverTime = new WaitForSeconds(0.5f);
    }

    public void SynchronizeLocomotion()
    {

    }

    public void AdjustRecoverTime(float adjustingRange)
    {
        combat.WaitRecoverTime = new WaitForSeconds(0.5f * adjustingRange);
    }

    public bool CompareDistanceWithoutHeight(Vector3 pos1, Vector3 pos2, float distance)
    {
        float f1 = pos1.x - pos2.x;
        float f2 = pos1.z - pos2.z;
        if (f1 * f1 + f2 * f2 > distance * distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckBackAttackChance()
    {        
        return Vector3.Dot(Camera.main.transform.forward, transform.position) <= 0f;
    }
}
