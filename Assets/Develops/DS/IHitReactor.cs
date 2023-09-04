using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumType;

public interface IHitReactor
{
    public void HitReact(HitTag[] hitType, int damage);
}
