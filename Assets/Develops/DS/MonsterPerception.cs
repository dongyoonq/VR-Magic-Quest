using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private IEnumerator CollapseRoutine()
    {
        // 죽는 애니메이션
        yield return null;
    }
}
