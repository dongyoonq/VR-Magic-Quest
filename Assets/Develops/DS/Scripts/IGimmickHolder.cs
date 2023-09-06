using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGimmickHolder
{
    bool isGimmickTriggered
    {
        get;
        set;
    }

    public IEnumerator GimmickRoutine();
}
