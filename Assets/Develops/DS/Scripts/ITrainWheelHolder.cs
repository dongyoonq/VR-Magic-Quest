using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static EnumType;

public interface ITrainWheelHolder
{
    public bool Brake
    { get; set; }

    public void Glide(Vector3 position);
    public void Turn(Vector3 trackDirection, float turnSpeed);
    public void Accelerate();
    public void Decelerate();
}
