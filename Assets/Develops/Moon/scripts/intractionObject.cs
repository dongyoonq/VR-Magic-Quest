using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class intractionObject : MonoBehaviour
{
    public Rigidbody objrb;
    public void Awake()
    {
        objrb = GetComponent<Rigidbody>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        //���������� �߷¹޾ƶ�������
        objrb.useGravity = true;

        //�������� �ı��ϰ� �ĸ��ν��Ͻ�ȭ�� �ް� �������� ƨ��Բ�����
        Destroy(this, 0.5f);
        
        //���� ������Ʈ ���̵� �ǵ���� ������
    }
}
