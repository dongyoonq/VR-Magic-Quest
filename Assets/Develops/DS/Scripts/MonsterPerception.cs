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

    // guard 타입은 놓치면 원래 자리로 복귀 aggressive는 적을 놓쳐도 해당 방향으로 일정 거리만큼 더 전진
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
        // 죽는 애니메이션
        // vr 상호작용 활성화
        // 임시 아이템 드롭 나중에 상호작용시 아이템으로 변하는 것으로 변경할 것
        if(monsterInfo.dropItems.Length > 0)
        {
            GameManager.Resource.Instantiate(monsterInfo.dropItems[Random.Range(0, monsterInfo.dropItems.Length)], true);
        }
        // 상호작용시 아이템으로 변함(아이템을 떨어트리고 pool 회수)
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
