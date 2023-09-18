using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class intractionObject : DestroyObject
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

    //�׽�Ʈ �뵵�� update ����
    public void Update()
    {
        if (Input.GetKey(KeyCode.K))
        {
            objrb.useGravity = true;
            
        }
    }
   //active�� false�� �ٴ��� ������ hitobject�Լ� ���Բ� ���
    public void OnCollisionEnter(Collision collision)
    {
        

        //�÷��̾� ��ų ������ gravity �ǰԲ� �����ϸ��
        if(collision.gameObject.layer == 9)
        {
            objrb.useGravity = true;
        }
        //�ٴڿ� ������ ����
        if (collision.gameObject.layer == 12)
        {

            skillHitObject();
        }
    }
    //�ٴڿ� �����鼭 ũ�Ⱑ �پ��鼭 �Ʒ��� �������Բ�����
    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            transform.position -= new Vector3(0, asd * Time.deltaTime, 0);

        }
    }

    public override void skillHitObject()
    {
        // ���ٴ��� ��������� ������ ����ؼ� �ִϸ��̼� ó����
        aim.SetBool("drop", true);
        ParticleSystem effect = Instantiate(hiteffect, transform.position, Quaternion.identity);
        Destroy(gameObject, 0.5f);
        Destroy(effect.gameObject, 1f);
    }

}
