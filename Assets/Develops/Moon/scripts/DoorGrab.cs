using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Primitives;

public class DoorGrab : XRBaseInteractable
{
   

    [SerializeField] public Transform Attachpoint;
    [SerializeField] GameObject rotateobj;
    [SerializeField] GameObject curattachpoint;
    IXRSelectInteractor interactor;
    IXRSelectInteractable curinteractable;


    [SerializeField] public bool ispos;
    [SerializeField] public Vector3 attachpos;

    public void Reset()
    {
        ispos = false;
        attachpos = Vector3.zero;

    }
    protected override void Awake()
    {
        base.Awake();
        Attachpoint = transform;
   
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
      //  Reset();
    }

    //이게 업데이트 개념
    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if(updatePhase== XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if (isSelected)
            {
                
                RotateUpdate();
            }
            if (!isSelected && ispos)
            {
                Debug.Log("리셋");
                Reset();
            }
        }
    }
    //갖다대기만해도 돌아감
    public void RotateUpdate()
    {
        //rotateobj.transform.Rotate(0, 0, 30*Time.deltaTime);  //이코드는 갖다 대기만 해도돌아
        dir();
    }
    public void dir()
    {
        if (!ispos)
        {
            attachpos = interactor.GetAttachTransform(this).position;
            ispos= true;
        }
        float ydis = interactor.GetAttachTransform(this).position.y - attachpos.y;
        Debug.Log(ydis);
        if (ydis < -0.1f)
        {
            rotateobj.transform.Rotate(0, 0, 30 * Time.deltaTime);
        }

    }
  
}
