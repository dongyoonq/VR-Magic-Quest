using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static EnumType;

public class MineCart : MonoBehaviour, ITrainWheelHolder
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float curveRate;
    private Coroutine skidRoutine;
    private WheelCollider[] wheels;
    private Rigidbody rigidbody;
    private bool brake;
    public bool Brake { get { return brake; } set {  brake = value; } }

    private void Awake()
    {
        wheels = GetComponentsInChildren<WheelCollider>();
        brake = true;
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Board();
    }

    public void Board()
    {
        skidRoutine = StartCoroutine(SkidRoutine());
    }

    private IEnumerator SkidRoutine()
    {
        WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
        brake = false;
        while (true)
        {
            foreach (WheelCollider wheel in wheels)
            {
                if (wheel.motorTorque > 0)
                {
                    wheel.motorTorque += -Time.deltaTime * speed;
                }
                yield return null;
            }
            yield return waitForFixedUpdate;
        }
    }

    public void Glide(Vector3 position)
    {
        Vector3 direction = (transform.position - position).normalized;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * curveRate);
    }

    public void Turn(Vector3 trackDirection, float turnSpeed)
    {
        Debug.Log(trackDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(trackDirection), turnSpeed * curveRate);
    }

    public void Accelerate()
    {
        if (brake)
        {
            return;
        }
        foreach (WheelCollider wheel in wheels)
        {
            wheel.motorTorque += Time.deltaTime * speed * 2f;
        }
    }

    public void Decelerate()
    {
        
    }
}
