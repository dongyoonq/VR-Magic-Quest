using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineCart : MonoBehaviour
{
    private Coroutine skidRoutine;
    private float gradient;
    private float currentSlope;
    private float onSteepTime;
    private float aceeleration;
    private Rigidbody rigidbody;
    [SerializeField]
    private LayerMask trackLayerMask;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        Board();
    }

    public void Board()
    {
        skidRoutine = StartCoroutine(SkidRoutine());
    }

    private IEnumerator SkidRoutine()
    {
        WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
        RaycastHit hitInfo;
        while (true)
        {
            if (Physics.Raycast(transform.position, -transform.up, out hitInfo ,0.1f, trackLayerMask))
            {
                gradient = Vector3.Dot(Vector3.up, hitInfo.normal);
                if (gradient < 0.9f)
                {
                    onSteepTime += Time.deltaTime;
                }
                currentSlope = Vector3.Dot(-transform.forward, hitInfo.normal);
            }
            else
            {
                onSteepTime = 0f;
            }
            Debug.Log(onSteepTime);
            yield return waitForFixedUpdate;
        }
    }
}
