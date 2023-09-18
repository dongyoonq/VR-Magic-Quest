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

    //테스트 용도로 update 만듬
    public void Update()
    {
        if (Input.GetKey(KeyCode.K))
        {
            objrb.useGravity = true;
            
        }
    }
   //active가 false된 바닥이 맞으면 hitobject함수 돌게끔 사용
    public void OnCollisionEnter(Collision collision)
    {
        

        //플레이어 스킬 맞으면 gravity 되게끔 설정하면됨
        if(collision.gameObject.layer == 9)
        {
            objrb.useGravity = true;
        }
        //바닥에 닿으면 시작
        if (collision.gameObject.layer == 12)
        {

            skillHitObject();
        }
    }
    //바닥에 닿으면서 크기가 줄어들면서 아래로 내려가게끔수정
    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            transform.position -= new Vector3(0, asd * Time.deltaTime, 0);

        }
    }

    public override void skillHitObject()
    {
        // 땅바닥이 만들어지는 느낌을 줘야해서 애니메이션 처리함
        aim.SetBool("drop", true);
        ParticleSystem effect = Instantiate(hiteffect, transform.position, Quaternion.identity);
        Destroy(gameObject, 0.5f);
        Destroy(effect.gameObject, 1f);
    }

}
