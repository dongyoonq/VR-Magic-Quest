using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonsterData;

public class MonsterPerception : MonoBehaviour
{
    enum BasicState
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
    private BasicState currentState;
    private IEnumerator basicAI;
    private IEnumerator advancedAI;

    public IEnumerator MakeDecisionRoutine()
    {
        Coroutine basicAIRoutine = StartCoroutine(basicAI);
        Coroutine advancedAIRoutine = StartCoroutine(advancedAI);
        yield return new WaitUntil(() => currentState == BasicState.Collapse);
        StopCoroutine(basicAIRoutine);
        StopCoroutine(advancedAIRoutine);
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
}
