using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEditor.PlayerSettings;

public class boxInteraction : XRSocketInteractor
{

    public string keytype;
    public bool acces= false;

    protected override void Awake()
    {
        base.Awake();
    }

 
    public override bool CanSelect(XRBaseInteractable interactable)
    {
        key key = interactable.GetComponent<key>();

        if (key == null)
            return false;

  
        return base.CanSelect(interactable) && (key.keytype == keytype);
    }
}
