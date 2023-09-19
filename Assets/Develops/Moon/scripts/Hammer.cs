using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public Rigidbody rb;
    [SerializeField] ParticleSystem hammerhiteffect;
    [SerializeField] GameObject obj;

    [SerializeField] float velocityx;
    [SerializeField] float velocityy;
    [SerializeField] float velocityz;
    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    //���� �ӷ��̻� �Ѿ���� �°Բ� ����
    public void Update()
    {
        velocityx = Mathf.Abs(obj.GetComponent<Rigidbody>().velocity.x);
        velocityy = Mathf.Abs(obj.GetComponent<Rigidbody>().velocity.y);
        velocityz = Mathf.Abs(obj.GetComponent<Rigidbody>().velocity.z);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Rock>() != null)
        {
            if(Addvelocity(velocityx, velocityy, velocityz) > 5)
            {
                Debug.Log("����");
                other.gameObject.GetComponent<Rock>().ToolHitObject();
            }
        }
    }
    public float Addvelocity(float a,float b,float c)
    {
        return a + b + c;
    }
}
