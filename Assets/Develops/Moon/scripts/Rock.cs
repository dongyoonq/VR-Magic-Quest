using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] public GameObject hitafter;
    [SerializeField] public ParticleSystem hiteffect;
    [SerializeField] public GameObject rockfragment;

    public void Awake()
    {
        hitafter.SetActive(false);
    }
    public void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
         
            Hit();
            
        }
    }

    public void Hit()
    {
        GameObject obj= Instantiate(hitafter, transform.position, Quaternion.identity);
        obj.SetActive(true);
        Destroy(this.gameObject);
        StartCoroutine(rockcreatRoutin());
        ParticleSystem effect= Instantiate(hiteffect, transform.position, Quaternion.identity);
        Destroy(effect.gameObject, 0.5f);
    }


    IEnumerator rockcreatRoutin()
    {
        int i = 0;
        while (i<10)
        {
            float randomnum = Random.Range(-5, 5);
            GameObject obj2 = Instantiate(rockfragment, transform.position, Quaternion.identity);
            obj2.GetComponent<Rigidbody>().AddForce(new Vector3(randomnum, randomnum, randomnum));
            Debug.Log(randomnum);
            i++;
        }
        yield return null;
    }
}
