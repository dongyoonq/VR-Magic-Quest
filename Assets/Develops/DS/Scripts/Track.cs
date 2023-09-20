using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumType;

public class Track : MonoBehaviour
{
    [SerializeField]
    private TrackDirection direction;
    private ITrainWheelHolder trainWheelHolder;
    public ITrainWheelHolder TrainWheelHolder { get { return trainWheelHolder; } }
    private Vector3 trackDirection;
    private float turnSpeed;

    private void Awake()
    {
        switch(direction)
        {
            case TrackDirection.Left:
                trackDirection = -transform.right;
                turnSpeed = Time.deltaTime;
                break;
            case TrackDirection.Right:
                trackDirection = -transform.forward;
                turnSpeed = Time.deltaTime;
                break;
            default:
                trackDirection = transform.forward;
                turnSpeed = Time.deltaTime * 0.5f;
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        trainWheelHolder = other.transform.root.GetComponentInChildren<ITrainWheelHolder>();
        trainWheelHolder.Accelerate();
        trainWheelHolder.Turn(trackDirection, turnSpeed);
    }
}
