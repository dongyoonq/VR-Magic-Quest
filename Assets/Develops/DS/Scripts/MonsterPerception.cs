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
    private BasicState currentState;
    public BasicState CurrentState { get { return currentState; } set { currentState = value; } }
    private IEnumerator advancedAI;

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
            case BasicState.Alert:
                vision.Gaze();
                break;
            case BasicState.Chase:
                vision.Gaze();
                break;
            case BasicState.Combat:
                vision.Gaze();
                combat.Combat();
                break;
            case BasicState.Flee:
                break;
            case BasicState.Collapse:
                break;
            default:
                break;
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
        currentState = BasicState.Idle;
        SynchronizeController((() => MonsterBasicBehave()), true);
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
}
