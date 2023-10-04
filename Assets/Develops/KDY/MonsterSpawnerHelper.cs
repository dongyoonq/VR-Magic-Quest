using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnerHelper : MonoBehaviour
{
    [SerializeField] List<Collider> spawnerTriggerList;
    [SerializeField] List<Vector3> spawnerPositionList;
    [SerializeField] List<MonsterPerception> spawnedMonster;

    private void Start()
    {
        InitRespawn();
    }

    private void InitRespawn()
    {
        foreach (Collider c in spawnerTriggerList)
        {
            c.enabled = true;
            spawnerPositionList.Add(c.transform.position);
        }
    }

    public void RespawnAll()
    {
        foreach (Collider c in spawnerTriggerList)
        {
            c.enabled = true;
        }
    }

    public void AddRespawnMonster(MonsterPerception mosnter)
    {
        spawnedMonster.Add(mosnter);
    }

    public void ResetMonsterSpawn()
    {
        foreach (MonsterPerception mosnter in spawnedMonster)
        {
            GameManager.Resource.Destroy(mosnter);
        }

        for (int i = 0; i < spawnerTriggerList.Count; i++)
        {
            spawnerTriggerList[i].transform.position = spawnerPositionList[i];
        }
    }
}