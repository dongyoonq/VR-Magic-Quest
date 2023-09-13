using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITrainWheelHolder
{
    public void Glide();
    public void Turn();
    public void Accelerate();
    public void Decelerate();
}
