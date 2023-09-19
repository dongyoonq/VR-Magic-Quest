using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;
using static EnumType;
using static UnityEngine.GraphicsBuffer;

public class MonsterPerception : MonoBehaviour
{
    private MonsterController controller;
    public MonsterController Controller { get { return controller; } }
    private MonsterController[] minionController;
    public MonsterController[] MinionController { get { return minionController; } set { minionController = value; } }
    private MonsterData.MonsterInfo monsterInfo;
    private MonsterVision vision;
    public MonsterVision Vision { get { return vision; } }
    private MonsterCombat combat;
    public MonsterCombat Combat { get { return combat; } }
    private MonsterLocomotion locomotion;
    public MonsterLocomotion Locomotion { get { return locomotion; } }
    private State currentState;
    public State CurrentState { get { return currentState; } set { currentState = value; } }
    private Condition currentCondition;
    public Condition CurrentCondition { get { return currentCondition; } set { currentCondition = value; } }
    private IEnumerator advancedAI;
    public GimmickTrigger GimmickTrigger { get { return controller.GimmickTrigger; } }
    [HideInInspector]
    public float alertMoveSpeed;
    [HideInInspector]
    public float chaseMoveSpeed;
    private Animator animator;
    [SerializeField]
    private string deathSound;

    private void Awake()
    {
        vision = GetComponent<MonsterVision>();
        combat = GetComponent<MonsterCombat>();
        locomotion = GetComponent<MonsterLocomotion>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        currentState = State.Idle;
        currentCondition = Condition.Normal;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public IEnumerator MakeDecisionRoutine()
    {
        yield return new WaitForSeconds(3f);
        advancedAI = monsterInfo.monsterAIRoutine;
        Coroutine advancedAIRoutine = StartCoroutine(advancedAI);
        yield return new WaitUntil(() => currentState == State.Collapse);
        StopCoroutine(advancedAIRoutine);
        yield return StartCoroutine(CollapseRoutine());
        GameManager.Resource.Destroy(gameObject);
    }

    public IEnumerator SpotEnemyRoutine(Transform enemyTransform)
    {
        currentState = State.Alert;
        controller.transform.position = enemyTransform.position;
        controller.transform.parent = enemyTransform;
        GameManager.Sound.PlayBGM("EnemyBGM");
        yield return null;
    }

    // guard 타입은 놓치면 원래 자리로 복귀 aggressive는 적을 놓쳐도 해당 방향으로 일정 거리만큼 더 전진
    public void LoseSightOfTarget()
    {
        currentState = State.Idle;
        if (controller == null)
        {
            return;
        }
        Player player = controller.transform.GetComponentInParent<Player>();
        controller.transform.parent = null;
        player.PlayerEndBattle();
        controller.transform.position = transform.position + transform.forward * 5f;
    }

    public void SendCommand(IEnumerator command)
    {
        controller.GetCommand(command);
    }

    public void ChangeCondition(int amount)
    {
        currentCondition += amount;
        if (currentCondition < 0)
        {
            currentCondition = Condition.Weak;
        }
        else if (currentCondition > Condition.TopForm)
        {
            currentCondition = Condition.TopForm;
        }
    }

    private void MonsterBasicBehave()
    {
        switch (currentState)
        {
            case State.Idle:
                vision.AvertEye();
                locomotion.SlowDown();
                break;
            case State.Alert:
                locomotion.Approach(alertMoveSpeed);
                locomotion.Turn();
                vision.Gaze();
                vision.CheckObstacle(vision.TargetTransform.position);
                break;
            case State.Chase:
                locomotion.Approach(chaseMoveSpeed);
                locomotion.Turn();
                vision.Gaze();
                vision.CheckObstacle(vision.TargetTransform.position);
                break;
            case State.Combat:
                vision.Gaze();
                locomotion.SlowDown();
                locomotion.Turn();
                combat.Combat();
                break;
            default:
                vision.AvertEye();
                locomotion.Stop();
                break;
        }
        if (currentState == State.Idle)
        {
            return;
        }
        if (CompareDistanceWithoutHeight(controller.transform.position, transform.position, monsterInfo.attackRange))
        {
            if (CheckBackAttackChance())
            {
                currentState = State.Chase;
            }
            else
            {
                currentState = State.Alert;
            }
        }
        else
        {
            transform.LookAt(controller.transform.position);
            currentState = State.Combat;
        }

    }

    public IEnumerator ReturnRoutine()
    {
        controller.transform.position = Locomotion.GuardPosition;
        locomotion.Moving = true;
        while (currentState == State.Idle && CompareDistanceWithoutHeight(transform.position, Locomotion.GuardPosition, 1f))
        {
            locomotion.Approach(alertMoveSpeed);
            locomotion.Turn();
            vision.Gaze();
            vision.CheckObstacle(vision.TargetTransform.position);
            yield return null;
        }
        locomotion.Moving = false;
    }

    public void DynamicallyMove()
    {
        alertMoveSpeed = Random.Range(monsterInfo.moveSpeed * 0.75f, monsterInfo.moveSpeed);
        chaseMoveSpeed = Random.Range(monsterInfo.moveSpeed * 0.75f, monsterInfo.moveSpeed);
    }

    private IEnumerator CollapseRoutine()
    {
        if (deathSound != null)
        {
            GameManager.Sound.PlaySFX(deathSound);
        }       
        CurrentState = State.Idle;
        LoseSightOfTarget();
        controller.UnlockNextArea();
        GameManager.Quest.KillMonster(monsterInfo.monsterName);
        locomotion.StopAllCoroutines();
        vision.StopAllCoroutines();
        combat.StopAllCoroutines();
        StopCoroutine(controller.monsterBehaviourRoutine);
        StopCoroutine(controller.monsterInvoluntaryBehaveRoutine);
        animator.SetBool("Collapse", true);
        animator.SetTrigger("GetHit");
        // 죽는 애니메이션
        // vr 상호작용 활성화
        // 임시 아이템 드롭 나중에 상호작용시 아이템으로 변하는 것으로 변경할 것

        // 임시 딜레이
        yield return new WaitForSeconds(3f);
        if(monsterInfo.dropItems.Length > 0)
        {
            GameManager.Resource.Instantiate(monsterInfo.dropItems[Random.Range(0, monsterInfo.dropItems.Length)], transform.position + Vector3.up, Quaternion.identity, true);
        }
        // 상호작용시 아이템으로 변함(아이템을 떨어트리고 pool 회수)
        yield return null;
    }

    public void ActivateMonster(MonsterController monsterController, MonsterData.MonsterInfo monsterInfo)
    {
        this.controller = monsterController;
        this.monsterInfo = monsterInfo;
        alertMoveSpeed = monsterInfo.moveSpeed;
        chaseMoveSpeed = monsterInfo.moveSpeed;
        currentState = State.Idle;
        SynchronizeController((() => MonsterBasicBehave()), true);
        SynchronizeController((() => locomotion.Fall()), false);
        SynchronizeController((() => locomotion.GroundCheck()), false);
        SynchronizeVision();
        SynchronizeCombat();
        SynchronizeLocomotion();
        advancedAI = monsterInfo.monsterAIRoutine;
        StartCoroutine(MakeDecisionRoutine());
    }

    public void SynchronizeController(UnityAction action, bool active)
    {
        if (active)
        {
            controller.activeEvent.AddListener(action);
        }
        else
        {
            controller.passiveEvent.AddListener(action);
        }
    }

    public void SynchronizeVision()
    {
        vision.DetectRange = monsterInfo.detectRange;
        WeightedTransformArray sourceObject = new WeightedTransformArray();
        sourceObject.Add(new WeightedTransform(controller.transform, 1f));
        vision.HeadAim.data.sourceObjects = sourceObject;
        vision.UpperBodyAIm.data.sourceObjects = sourceObject;
        vision.RigBuilder.Build();
    }

    public void SynchronizeCombat()
    {
        combat.Stat = (monsterInfo.healthPoint, monsterInfo.attackPoint, monsterInfo.attackSpeed);
        combat.MaxHP = monsterInfo.healthPoint;
        combat.WaitRecoverTime = new WaitForSeconds(0.5f);
    }

    public void SynchronizeLocomotion()
    {
        locomotion.targetTransform = controller.transform;
    }

    public void AdjustRecoverTime(float adjustingRange)
    {
        combat.WaitRecoverTime = new WaitForSeconds(0.5f * adjustingRange);
    }

    public bool CompareDistanceWithoutHeight(Vector3 pos1, Vector3 pos2, float distance)
    {
        float f1 = pos1.x - pos2.x;
        float f2 = pos1.z - pos2.z;
        if (f1 * f1 + f2 * f2 > distance * distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckBackAttackChance()
    {        
        return Vector3.Dot(Camera.main.transform.forward, transform.position) <= 0f;
    }

    public void SpinMonsterController(bool spinLeft)
    {
        if(currentState == State.Idle)
        {
            if (spinLeft)
            {
                controller.transform.RotateAround(transform.position, Vector3.down, Time.deltaTime * 60f);
            }
            else
            {
                controller.transform.RotateAround(transform.position, Vector3.up, Time.deltaTime * 60f);
            }
        }
    }
}
