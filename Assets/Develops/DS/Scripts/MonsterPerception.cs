using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;
using static EnumType;
using static UnityEngine.GraphicsBuffer;

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
    [HideInInspector]
    public float alertMoveSpeed;
    [HideInInspector]
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
        //controller.transform.parent = null;
        //controller.transform.position = transform.position + transform.forward * 5f;
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
                vision.AvertEye();
                locomotion.SlowDown();
                break;
            case BasicState.Alert:
                locomotion.Approach(alertMoveSpeed);
                locomotion.Turn();
                vision.Gaze();
                break;
            case BasicState.Chase:
                locomotion.Approach(chaseMoveSpeed);
                locomotion.Turn();
                vision.Gaze();
                break;
            case BasicState.Combat:
                vision.Gaze();
                locomotion.SlowDown();
                locomotion.Turn();
                combat.Combat();
                break;
            default:
                vision.AvertEye();
                locomotion.Stop();
                break;
        }
        if (currentState == BasicState.Idle)
        {
            return;
        }
        if (CompareDistanceWithoutHeight(controller.transform.position, transform.position, monsterInfo.attackRange))
        {
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

    public void DynamicallyMove()
    {
        alertMoveSpeed = Random.Range(monsterInfo.moveSpeed * 0.75f, monsterInfo.moveSpeed);
        chaseMoveSpeed = Random.Range(monsterInfo.moveSpeed * 0.75f, monsterInfo.moveSpeed);
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
        SynchronizeLocomotion();
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
        WeightedTransformArray sourceObject = new WeightedTransformArray();
        sourceObject.Add(new WeightedTransform(controller.transform, 1f));
        vision.HeadAim.data.sourceObjects = sourceObject;
        vision.UpperBodyAIm.data.sourceObjects = sourceObject;
        vision.RigBuilder.Build();
    }

    public void SynchronizeCombat()
    {
        combat.Stat = (monsterInfo.healthPoint, monsterInfo.attackPoint);
        combat.WaitRecoverTime = new WaitForSeconds(0.5f);
    }

    public void SynchronizeLocomotion()
    {
        locomotion.targetTransform = controller.transform;
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