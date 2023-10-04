using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ActiveObj : MonoBehaviour
{
    public Renderer render;
    public Material material;
    [SerializeField] public float basealpha;
    [SerializeField] public float maxalpha;
    public bool isac;
    TeleportationArea tparea;
    public void Awake()
    {
        render = GetComponent<Renderer>();
        material = render.material;
        tparea = GetComponent<TeleportationArea>();
        tparea.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {

        //종유석맞으면
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
            tparea.enabled = true;
            yield return null;
        }
        yield return null;
    }
}
