using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ha : XRGrabInteractable
{
    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        SetParentToXRRig();
        base.OnSelectEntered(interactor);
    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        SetParentToWorld();
        base.OnSelectExited(interactor);
    }

    public void SetParentToXRRig()
    {
        transform.SetParent(selectingInteractor.transform);
        transform.localScale = new Vector3(1, 1, 1);
    }

    public void SetParentToWorld()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }
}
