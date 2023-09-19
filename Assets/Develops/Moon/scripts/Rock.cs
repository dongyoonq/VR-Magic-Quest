using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Rock : DestroyObject
{
    [SerializeField] public GameObject hitafter;
    [SerializeField] public ParticleSystem hiteffect;
    [SerializeField] public GameObject rockfragment;
    [SerializeField] public int hitcount;
    [SerializeField] public string recipe;
    public void Awake()
    {
        hitcount = 0;
        hitafter.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        //skill ¸ÂÀ¸¸é
      if(collision.gameObject.layer==9)
        {
            skillHitObject();
        }
    }
    public override void skillHitObject()
    {
        DestroyObject();

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
        GameManager.Sound.PlaySFX("BreakStone");
        GameObject recipeResource = GameManager.Resource.Instantiate<GameObject>($"Prefabs/Recipe/{recipe}", transform.position + transform.up * 1.5f, Quaternion.identity);

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
