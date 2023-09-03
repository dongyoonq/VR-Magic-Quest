using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorGrab : XRBaseInteractable
{
   

    [SerializeField] public Transform Attachpoint;
    [SerializeField] GameObject rotateobj;
    [SerializeField] GameObject curattachpoint;
    IXRSelectInteractor interactor;
    IXRSelectInteractable curinteractable;
    Transform ratateobjbasepos;

    public void Reset()
    {
        StartCoroutine(restart());
    }
    protected override void Awake()
    {
        base.Awake();
        Attachpoint = transform;
        ratateobjbasepos = rotateobj.transform;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        selectEntered.AddListener(StartGrab);
        selectExited.AddListener(EndGrab);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        selectEntered.RemoveListener(StartGrab);
        selectExited.RemoveListener(EndGrab);

    }

    public void StartGrab(SelectEnterEventArgs args)
    {
        
        interactor = args.interactorObject;
        curinteractable = args.interactableObject;
    }

    public void EndGrab(SelectExitEventArgs args)
    {
        interactor = null;
        Reset();
    }


    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if(updatePhase== XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if (isSelected)
            {
                
                RotateUpdate();
            }
        }
    }

    public void RotateUpdate()
    {
        rotateobj.transform.Rotate(0, 0, 10 * Time.deltaTime);


    }
    IEnumerator restart()
    {
        float pos = Vector3.Distance(ratateobjbasepos.position, rotateobj.transform.position);

        while (pos > 0)
        {
            rotateobj.transform.Rotate(0, 0, -10 * Time.deltaTime);
        }
        yield return null;
    }
}
