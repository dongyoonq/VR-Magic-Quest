using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using static EnumType;

public class MonsterVision : MonoBehaviour
{
    [SerializeField]
    private Transform eyeTransform;
    [Range(0, 360)]
    [SerializeField]
    private float fieldOfView;
    [SerializeField]
    private RigBuilder rigBuilder;
    public RigBuilder RigBuilder { get { return rigBuilder; } }
    [SerializeField]
    private MultiAimConstraint headAim;
    public MultiAimConstraint HeadAim { get {  return headAim; } }
    [SerializeField]
    private MultiAimConstraint upperBodyAim;
    public MultiAimConstraint UpperBodyAIm { get { return upperBodyAim; } }
    private MonsterPerception perception;
    private float detectRange;
    public float DetectRange {  get { return detectRange; } set { detectRange = value; } }
    private Vector3 targetDirection;
    private LayerMask detectLayerMask;
    private (Transform targetTransform, float targetSqrDistance) target;
    private Transform targetTransform;
    public Transform TargetTransform { get { return targetTransform; } set { targetTransform = value; } }

    private void Awake()
    {
        perception = GetComponent<MonsterPerception>();
        detectLayerMask = -1;
        detectLayerMask &= ~(LayerMask.GetMask("Monster") | LayerMask.GetMask("Trigger"));
        headAim.weight = 0f;
        upperBodyAim.weight = 0f;
    }

    private void OnEnable()
    {
        StartCoroutine(VisionRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        perception.LoseSightOfTarget();
    }

    public IEnumerator VisionRoutine()
    {
        yield return null;      
        while (true)
        {
            target = (Camera.allCameras[0].transform, Vector3.SqrMagnitude(Camera.allCameras[0].transform.position - transform.position));
            foreach (Camera player in Camera.allCameras)
            {
                float sqrDistance = Vector3.SqrMagnitude(player.transform.position - transform.position);
                if (sqrDistance < target.targetSqrDistance)
                {
                    target = (player.transform, sqrDistance);
                }
                yield return null;
            }
            if (target.targetSqrDistance < detectRange * detectRange)
            {
                targetDirection = (target.targetTransform.position - eyeTransform.position).normalized;
                if (Vector3.Dot(transform.forward, targetDirection) >= Mathf.Cos(fieldOfView * 0.5f * Mathf.Deg2Rad))
                {
                    CheckObstacle(target.targetTransform.position);
                }
            }
            else
            {
                perception.LoseSightOfTarget();
            }
            yield return null;
        }
    }

    public void Gaze()
    {
        if (headAim.weight < 1f)
        {
            headAim.weight = Mathf.Lerp(headAim.weight, 1f, Time.deltaTime);
        }
        if (upperBodyAim.weight < 1f)
        {
            upperBodyAim.weight = Mathf.Lerp(upperBodyAim.weight, 1f, Time.deltaTime);
        }
    }

    public void AvertEye()
    {
        if (headAim.weight > 0f)
        {
            headAim.weight = Mathf.Lerp(headAim.weight, 0f, Time.deltaTime);
        }
        if (upperBodyAim.weight > 0f)
        {
            upperBodyAim.weight = Mathf.Lerp(upperBodyAim.weight, 0f, Time.deltaTime);
        }
    }

    public void CheckBehind()
    {

    }

    public void CheckObstacle(Vector3 targetPosition)
    {
        RaycastHit hitInfo;
        if (Physics.Linecast(eyeTransform.position, targetPosition, out hitInfo, detectLayerMask))
        {
            if (hitInfo.collider.gameObject.layer != 7)
            {
                if (perception.CurrentState != State.Idle)
                {
                    perception.Locomotion.ComeRound(targetPosition, Vector3.Dot(hitInfo.transform.position - transform.position, transform.right) > 0);
                }
            }
            else
            {
                if (targetTransform != target.targetTransform)
                {
                    targetTransform = target.targetTransform;
                    perception.SendCommand(perception.SpotEnemyRoutine(targetTransform.parent.parent));
                }
            }
        }
        
    }
}
