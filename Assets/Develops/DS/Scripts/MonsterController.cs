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
        spawnTriggerCollider = GetComponent<Collider>();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator MonsterBehaveRoutine()
    {
        yield return new WaitForSeconds(3f);
        while (true)
        {
            if (commandQueue.Count > 0)
            {
                yield return StartCoroutine(commandQueue.Dequeue());
            }
            activeEvent?.Invoke();
            yield return null;
        }
    }

    private IEnumerator MonsterInvoluntaryBehaveRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        while (true)
        {
            passiveEvent?.Invoke();
            yield return null;
        }
    }

    public void GetCommand(IEnumerator command)
    {
        commandQueue.Enqueue(command);
    }

    public void SpawnMonster(int monsterNumber, Transform spawnPoint)
    {
        MonsterData.MonsterInfo monsterInfo = monsterData.MonsterType[monsterNumber];
        monsterPerception = GameManager.Resource.Instantiate(monsterInfo.monsterPrefab, spawnPoint.position + Vector3.up * 0.5f, spawnPoint.rotation, true).GetComponent<MonsterPerception>();
        StartCoroutine(MonsterInvoluntaryBehaveRoutine());
        monsterData.SynchronizeAI(ref monsterInfo, monsterPerception);
        monsterPerception.ActivateMonster(this, monsterInfo);
        monsterBehaviourRoutine = StartCoroutine(MonsterBehaveRoutine());
    }

    //TODO: �÷��̾� ���̾ ������ �ٸ� �ݸ������� �ƿ� �����ϵ��� projectsetting
    private void OnTriggerEnter(Collider other)
    {
        SpawnMonster(spawnMonsterNumber, spawnPoint);
        spawnTriggerCollider.enabled = false;
    }
}
