using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MonsterLocomotion : MonoBehaviour
{
    private Transform monsterControllerTransform;
    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void Approach(float moveSpeed)
    {
        
    }

    //public void Move()
    //{
    //    if (CompareDistanceWithoutHeight(objectivePoint, transform.position, 0.001f))
    //    {
    //        moveDirection = (objectivePoint - transform.position).normalized;
    //        //KeepPace();
    //        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
    //        animator.SetFloat("MoveSpeed", moveSpeed);
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * 10f);
    //        Fall();
    //    }
    //    // ���� �� ��ġ�ؾ��ϴ� �������� ����ؼ� �����̰�
    //    // �׸��� ��Ʈ�ѷ����� state�� ���� �����̴� �� ���߰� �����̳� ��� �ൿ�� �ڷ�ƾ���� �����Ѵ�
    //}

    //IEnumerator MovetoPositonRoutine(Vector3 positon)
    //{
    //    float elapsedTime = 0f;
    //    outofControl = true;
    //    Vector3 direction = (positon - transform.position).normalized;
    //    float speed = moveSpeed > walkSpeed ? moveSpeed : walkSpeed;
    //    animator.SetFloat("MoveSpeed", speed);
    //    float dot = Vector3.Dot(transform.forward, direction);
    //    while (true)
    //    {
    //        if ((positon - transform.position).sqrMagnitude < 0.05f)
    //        {
    //            break;
    //        }
    //        if (elapsedTime > 3f)
    //        {
    //            transform.Rotate(-transform.forward);
    //            StartCoroutine(MovetoPositonRoutine(positon));
    //        }
    //        if (1f - dot > 0.01f)
    //        {
    //            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 2.5f);
    //            dot = Vector3.Dot(transform.forward, direction);
    //        }
    //        controller.Move(direction * Time.deltaTime * speed);
    //        yield return null;
    //    }
    //    transform.Translate(positon);
    //    moveDirection.x = 0;
    //    moveDirection.z = 0;
    //    outofControl = false;
    //    yield return null;
    //}
}
