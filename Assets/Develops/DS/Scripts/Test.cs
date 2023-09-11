using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Test : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
    [SerializeField]
    MultiAimConstraint headAim;
    [SerializeField]
    private MultiAimConstraint bodyAim;
    RigBuilder rb;

    private void Start()
    {
        rb = GetComponent<RigBuilder>();
        WeightedTransformArray sourceObject = new WeightedTransformArray();
        sourceObject.Add(new WeightedTransform(target.transform, 1f));
        headAim.data.sourceObjects = sourceObject;
        bodyAim.data.sourceObjects = sourceObject;
        rb.Build();
    }
}
