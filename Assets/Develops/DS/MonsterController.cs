using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterController : MonoBehaviour
{
    [SerializeField]
    private MonsterData monsterData;
    public UnityEvent passiveEvent;
    public UnityEvent activeEvent;
    private Queue<IEnumerator> commandQueue = new Queue<IEnumerator>();
    MonsterData.MonsterInfo monsterInfo;

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
        monsterInfo = monsterData.MonsterInfo[monsterNumber];
        monsterData.SynchronizeAI(monsterInfo);
        GameManager.Resource.Instantiate(monsterInfo.monster, spawnPosition, Quaternion.identity, true);
    }
}
