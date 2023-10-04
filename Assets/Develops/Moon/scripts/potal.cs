using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class potal : MonoBehaviour
{
    [SerializeField] MonsterSpawnerHelper monsterSpawnerHelper;
    [SerializeField] GameObject potalstart;
    [SerializeField] GameObject potalend;
    [SerializeField] GameObject potalUI;
   [SerializeField] GameObject playerobj;


    private void Awake()
    {
        potalUI.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            potalUI.SetActive(true);
            playerobj = other.gameObject;

        }

      //  Debug.Log("¥Í¿Ω");
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            potalUI.SetActive(false);
            playerobj = null;
 
        }
    }

    public void  potalMove()
    {
        monsterSpawnerHelper.RespawnAll();
        GameManager.Sound.PlayBGM("DungeonBGM");
        playerobj.transform.position = potalend.transform.position+ potalend.transform.up*1.5f;
    }
}
