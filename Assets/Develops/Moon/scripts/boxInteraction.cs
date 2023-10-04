using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEditor.PlayerSettings;

public class boxInteraction : XRSocketInteractor
{
    public string keytype;
    public bool access = false;
    private box box;
    private key key;

    protected override void Awake()
    {
        box = GetComponentInParent<box>();
        base.Awake();
    }

 
    public override bool CanSelect(XRBaseInteractable interactable)
    {
        key key = interactable.GetComponent<key>();

        if (key == null)
            return false;

        this.key = key;
        access = base.CanSelect(interactable) && (key.keytype == keytype);

        return base.CanSelect(interactable) && (key.keytype == keytype);
    }

    
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (access)
        {
            box.Open();
            key.Use();
        }
    }
}
