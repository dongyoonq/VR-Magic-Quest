using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using static EnumType;

public class MonsterVision : MonoBehaviour
{
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

    private void Awake()
    {
        perception = GetComponent<MonsterPerception>();
        detectRange = GetComponent<SphereCollider>();
    }

    public void Gaze()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7 && perception.CurrentState == BasicState.Idle)
        {
            perception.SpotEnemy(other.transform);
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
