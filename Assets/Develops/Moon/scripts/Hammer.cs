using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public Rigidbody rb;
    [SerializeField] ParticleSystem hammerhiteffect;
    [SerializeField] Vector3 sp;
    [SerializeField] GameObject obj;
    [SerializeField] float spped;
    public void Awake()
    {
        
        rb = GetComponent<Rigidbody>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Rock>() != null)
        {
           
            other.gameObject.GetComponent<Rock>().ToolHitObject();
        }
    }
}
