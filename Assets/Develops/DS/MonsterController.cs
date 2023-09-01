using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterController : MonoBehaviour
{
    [SerializeField]
    private MonsterData monsterData;
    private MonsterPerception monsterPerception;
    public UnityEvent passiveEvent;
    public UnityEvent activeEvent;
    private Queue<IEnumerator> commandQueue = new Queue<IEnumerator>();

    private IEnumerator PlayerBehaveRoutine()
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
        MonsterData.MonsterInfo monsterInfo = monsterData.MonsterSpecies[monsterNumber];
        monsterData.SynchronizeAI(ref monsterInfo);
        monsterPerception = GameManager.Resource.Instantiate(monsterInfo.monster, spawnPosition, Quaternion.identity, true).GetComponent<MonsterPerception>();
        monsterPerception.ActivateMonster(monsterInfo);
    }
}
