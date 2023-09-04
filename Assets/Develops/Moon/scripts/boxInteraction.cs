using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class boxInteraction : XRSocketInteractor
{

    public string keytype;
    public bool acces= false;

    protected override void Awake()
    {
        base.Awake();
    }

}
