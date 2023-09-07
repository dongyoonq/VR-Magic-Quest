using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class ActiveObj : MonoBehaviour
{
    public Renderer render;
    public Material material;
    [SerializeField] public float basealpha;
    [SerializeField] public float maxalpha;
    public bool isac;
    public void Awake()
    {
        render = GetComponent<Renderer>();
        material = render.material;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 8)
        {
          //  isac = true;
            
          //  material.SetFloat("_Alpha", sp);
            StartCoroutine(activestart());
        }
    }

    IEnumerator activestart()
    {
        
        while (basealpha<maxalpha)
        {
            basealpha += Time.deltaTime;
            material.SetFloat("_Alpha", basealpha);
            yield return null;
        }
        yield return null;
    }
}
