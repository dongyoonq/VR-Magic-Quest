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
        //마법맞으면 중력받아떠러지고
        objrb.useGravity = true;

        //땅맞으면 파괴하고 파면인스턴스화로 받고 여러방향 튕기게끔생성
        Destroy(this, 0.5f);
        
        //맞은 오브젝트 쉐이드 건드려서 땅생성
    }
}
