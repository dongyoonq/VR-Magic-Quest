using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class intractionObject : MonoBehaviour
{
    public Rigidbody objrb;
    public Animator aim;
    [SerializeField] float asd;
    [SerializeField] ParticleSystem hiteffect;
    public void Awake()
    {
        objrb = GetComponent<Rigidbody>();
        aim = GetComponent<Animator>();
     
    }

    //�����ڽ����� �δ���
    public void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            objrb.useGravity = true;
            
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        //���������� �߷¹޾ƶ�������
        objrb.useGravity = true;

        if (collision.gameObject.layer == 9)
        {
            hitobj();
        }
    }
    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            transform.position -= new Vector3(0, asd * Time.deltaTime, 0);

        }
    }

    public void hitobj()
    {
        aim.SetBool("drop", true);
        ParticleSystem effect = Instantiate(hiteffect, transform.position, Quaternion.identity);
        Destroy(gameObject, 0.5f);
        Destroy(effect.gameObject, 1f);
    }
}
