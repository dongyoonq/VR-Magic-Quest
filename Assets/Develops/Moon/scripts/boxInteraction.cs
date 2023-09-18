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

    public override void PreprocessInteractor(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.PreprocessInteractor(updatePhase);

        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if (acces)
            {
                Debug.Log("¿¬°áµÊ");
                var interactor = GetComponent<XRBaseInteractor>();

                transform.GetChild(1).transform.rotation = interactor.gameObject.transform.rotation;
            }
        }
    }
    public override bool CanSelect(XRBaseInteractable interactable)
    {
        key key = interactable.GetComponent<key>();

        if (key == null)
            return false;

        acces = true;
        return base.CanSelect(interactable) && (key.keytype == keytype);
    }
}
