using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MonsterLocomotion : MonoBehaviour
{
    [HideInInspector]
    public Transform targetTransform;
    private CharacterController characterController;
    private Animator animator;
    private float ySpeed;
    private bool floating;
    private float floatingTime;
    private bool spellCaster;
    public bool SpellCaster { get { return spellCaster; } set {  spellCaster = value; } }
    private bool eliteMonster;
    public bool EliteMonster { get {  return eliteMonster; } set {  eliteMonster = value; } }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        ySpeed = 0f;
        animator.SetFloat("MoveSpeed", 0f);
        spellCaster = false;
        eliteMonster = false;
    }

    public void Approach(float moveSpeed)
    {
        animator.SetFloat("MoveSpeed", Mathf.Lerp(animator.GetFloat("MoveSpeed"), moveSpeed, Time.deltaTime));
        characterController.Move(transform.forward * animator.GetFloat("MoveSpeed") * Time.deltaTime * 0.5f);
    }

    public IEnumerator RushRoutine(float moveSpeed, float rushTime)
    {
        // ���� �غ� �ִϸ��̼�
        if (eliteMonster)
        {
            // �޸��� Turn ����
            // �� �տ��� ���߱�
            // ������ ����
        }
        else
        {
            // �޸��� ó�� �÷��̾� �������� ����
            // �����Ÿ���ŭ ����
            // ������ �÷��̾� �������� ȸ��
        }
        float time = 0f;
        animator.SetFloat("MoveSpeed", moveSpeed);
        while (time < rushTime)
        {
            Turn(); Turn(); Turn();
            characterController.Move(transform.forward * moveSpeed * Time.deltaTime * 0.5f);
            time += Time.deltaTime;
            yield return null;
        }
        animator.SetFloat("MoveSpeed", 0f);
    }

    public void SlowDown()
    {
        animator.SetFloat("MoveSpeed", Mathf.Lerp(animator.GetFloat("MoveSpeed"), 0f, Time.deltaTime * 5f));
    }

    public void Stop()
    {
        animator.SetFloat("MoveSpeed", 0f);
    }

    public void Turn()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetTransform.position - transform.position), Time.deltaTime);
    }

    public void Fall()
    {
        ySpeed += Physics.gravity.y * Time.deltaTime;
        if (!floating && ySpeed < 0)
        {
            ySpeed = -1f;
        }
        characterController.Move(Vector3.up * ySpeed * Time.deltaTime * 2f);
    }

    public IEnumerator DodgeRoutine()
    {
        animator.SetBool("Dodge", true);
        if (spellCaster)
        {

        }
        else
        {

        }
        yield return null;
    }

    public IEnumerator ShovedRoutine(int shovedPower)
    {
        for (int i = 0; i < shovedPower; i++)
        {
            characterController.Move(-(Camera.main.transform.position - transform.position).normalized * Time.deltaTime * 10f);
            yield return null;
        }       
    }

    public void GroundCheck()
    {
        floatingTime += Time.deltaTime;
        if (floatingTime > 0.1f)
        {
            floating = true;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        floating = false;
        floatingTime = 0f;
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
