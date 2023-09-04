using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPerception : MonoBehaviour
{
    public enum BasicState
    {
        Idle,
        Alert,
        Chase,
        Combat,
        Flee,
        Collapse
    }

    enum AdvancedState
    {

    }

    private MonsterController monsterController;
    private MonsterData.MonsterInfo monsterInfo;
    private BasicState currentState;
    public BasicState CurrentState { get { return currentState; } set { currentState = value; } }
    private IEnumerator basicAI;
    private IEnumerator advancedAI;

    public IEnumerator MakeDecisionRoutine()
    {
        basicAI = monsterInfo.monsterBasicAI;
        advancedAI = monsterInfo.monsterAdvancedAI;
        Coroutine basicAIRoutine = StartCoroutine(basicAI);
        //Coroutine advancedAIRoutine = StartCoroutine(advancedAI);
        yield return new WaitUntil(() => currentState == BasicState.Collapse);
        StopCoroutine(basicAIRoutine);
        //StopCoroutine(advancedAIRoutine);
        yield return StartCoroutine(CollapseRoutine());
        GameManager.Resource.Destroy(gameObject);
    }

    public void SpotEnemy(Transform enemyTransform)
    {
        currentState = BasicState.Alert;
        monsterController.transform.position = enemyTransform.position;
        monsterController.transform.parent = enemyTransform;
    }

    private IEnumerator CollapseRoutine()
    {
        // 죽는 애니메이션
        yield return null;
    }

    public void ActivateMonster(MonsterData.MonsterInfo monsterInfo)
    {
        this.monsterInfo = monsterInfo;
        currentState = BasicState.Idle;
        basicAI = monsterInfo.monsterBasicAI;
        //advancedAI = monsterInfo.monsterAdvancedAI;
        StartCoroutine(MakeDecisionRoutine());
    }

    public void Test()
    {
        Debug.Log("Test");
    }
}
