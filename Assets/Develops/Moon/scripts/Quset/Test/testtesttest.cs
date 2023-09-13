using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testtesttest : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            GameManager.Quest.KillMonster(gameObject.name);
            Debug.Log(gameObject.name);
            Destroy(gameObject);
            
        }
    }
}
