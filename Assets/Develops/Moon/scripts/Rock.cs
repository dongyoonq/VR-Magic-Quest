using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Rock : DestroyObject
{
    [SerializeField] public GameObject hitafter;
    [SerializeField] public ParticleSystem hiteffect;
    [SerializeField] public GameObject rockfragment;
    [SerializeField] public int hitcount;
    public void Awake()
    {
        hitcount = 0;
        hitafter.SetActive(false);
    }
    public void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {

            DestroyObject();
            
        }
    }

    public override void skillHitObject()
    {
        DestroyObject();
          /*  GameObject obj = Instantiate(hitafter, transform.position, Quaternion.identity);
            obj.SetActive(true);
            Destroy(this.gameObject);
            StartCoroutine(rockcreatRoutin());
            ParticleSystem effect = Instantiate(hiteffect, transform.position, Quaternion.identity);
            Destroy(effect.gameObject, 0.5f);*/

    }
    public override void ToolHitObject()
    {
        hitcount++;
        if (hitcount > 2)
        {
            DestroyObject();
        }


    }

    public void DestroyObject()
    {
        GameObject obj = Instantiate(hitafter, transform.position, Quaternion.identity);
        obj.SetActive(true);
        Destroy(this.gameObject);
        StartCoroutine(rockcreatRoutin());
        ParticleSystem effect = Instantiate(hiteffect, transform.position, Quaternion.identity);
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
         
            i++;
        }
        yield return null;
    }
}
