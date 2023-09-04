using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterController : MonoBehaviour
{
    [SerializeField]
    private MonsterData monsterData;
    [SerializeField]
    private int testSpawnMonsterNumber;
    private MonsterPerception monsterPerception;
    public UnityEvent passiveEvent = new UnityEvent();
    public UnityEvent activeEvent = new UnityEvent();
    private Queue<IEnumerator> commandQueue = new Queue<IEnumerator>();

    private void Start()
    {
        SpawnMonster(testSpawnMonsterNumber, transform.position + (-transform.forward *5f));
    }

    private IEnumerator MonsterBehaveRoutine()
    {
        yield return null;
        while (true)
        {
            passiveEvent?.Invoke();
            if (commandQueue.Count > 0)
            {
                yield return StartCoroutine(commandQueue.Dequeue());
            }
            activeEvent?.Invoke();
            yield return null;
        }
    }

    public void GetCommand(IEnumerator command)
    {
        commandQueue.Enqueue(command);
    }

    public void SpawnMonster(int monsterNumber, Vector3 spawnPosition)
    {
        MonsterData.MonsterInfo monsterInfo = monsterData.MonsterType[monsterNumber];
        monsterPerception = GameManager.Resource.Instantiate(monsterInfo.monsterPrefab, spawnPosition, Quaternion.identity, true).GetComponent<MonsterPerception>();
        monsterData.SynchronizeAI(ref monsterInfo, monsterPerception);
        monsterPerception.ActivateMonster(this, monsterInfo);
        StartCoroutine(MonsterBehaveRoutine());
    }
}
