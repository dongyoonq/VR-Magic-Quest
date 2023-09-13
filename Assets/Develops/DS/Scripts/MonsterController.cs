using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterController : MonoBehaviour
{
    [SerializeField]
    private MonsterData monsterData;
    [SerializeField]
    private int spawnMonsterNumber;
    private MonsterPerception monsterPerception;
    public UnityEvent passiveEvent = new UnityEvent();
    public UnityEvent activeEvent = new UnityEvent();
    private Queue<IEnumerator> commandQueue = new Queue<IEnumerator>();
    public Coroutine monsterBehaviourRoutine;
    private Transform spawnPoint;
    private Collider spawnTriggerCollider;

    private void Awake()
    {
        spawnPoint = transform.GetChild(0).transform;
        spawnTriggerCollider = GetComponentInChildren<Collider>();
        //Test
        SpawnMonster(spawnMonsterNumber, spawnPoint.position);
    }

    private IEnumerator MonsterBehaveRoutine()
    {
        yield return new WaitForSeconds(3f);
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
        monsterPerception = GameManager.Resource.Instantiate(monsterInfo.monsterPrefab, spawnPosition + Vector3.up * 0.5f, Quaternion.identity, true).GetComponent<MonsterPerception>();
        monsterData.SynchronizeAI(ref monsterInfo, monsterPerception);
        monsterPerception.ActivateMonster(this, monsterInfo);
        monsterBehaviourRoutine = StartCoroutine(MonsterBehaveRoutine());
    }

    //TODO: 플레이어 레이어를 제외한 다른 콜리더들은 아예 무시하도록 projectsetting
    private void OnTriggerEnter(Collider other)
    {
        SpawnMonster(spawnMonsterNumber, spawnPoint.position);
        spawnTriggerCollider.enabled = false;
    }
}
