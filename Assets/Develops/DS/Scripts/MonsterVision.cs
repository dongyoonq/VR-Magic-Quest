using System.Collections;
using System.Collections.Generic;
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
    private SphereCollider detectRange;
    public SphereCollider DetectRange {  get { return detectRange; } set { detectRange = value; } }
    private Vector3 targetDirection;
    private LayerMask detectLayerMask;

    private void Awake()
    {
        perception = GetComponent<MonsterPerception>();
        detectRange = GetComponent<SphereCollider>();
        detectLayerMask = -1;
        detectLayerMask &= ~(LayerMask.GetMask("Monster") | LayerMask.GetMask("Trigger"));
        headAim.weight = 0f;
        upperBodyAim.weight = 0f;
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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 7 && perception.CurrentState == BasicState.Idle)
        {
            targetDirection = (other.transform.position - eyeTransform.position).normalized;
            if (Vector3.Dot(transform.forward, targetDirection) >= Mathf.Cos(fieldOfView * 0.5f * Mathf.Deg2Rad))
            {
                RaycastHit hitInfo;
                Physics.Raycast(eyeTransform.position, targetDirection, out hitInfo, detectRange.radius, detectLayerMask);
                if (hitInfo.collider.gameObject.layer == 7)
                {
                    perception.SpotEnemy(other.transform);
                }       
            }           
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            perception.LoseSightOfTarget();
        }
    }
}
