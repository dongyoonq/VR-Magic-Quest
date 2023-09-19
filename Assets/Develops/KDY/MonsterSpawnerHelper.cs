using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnerHelper : MonoBehaviour
{
    [SerializeField] List<Collider> spawnerTriggerList;
    [SerializeField] List<MonsterPerception> spawnedMonster;

    public void RespawnAll()
    {


        foreach (Collider c in spawnerTriggerList)
        {
            c.enabled = true;
        }
    }
}