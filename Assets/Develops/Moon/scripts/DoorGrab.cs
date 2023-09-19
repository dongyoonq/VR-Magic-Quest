using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] public Door door;
    [SerializeField] public float ydis;

    [SerializeField] public float pos;


    public void Reset()
    {
        ispos = false;
        attachpos = Vector3.zero;

    }
    protected override void Awake()
    {
        base.Awake();
        Attachpoint = transform;
        door = transform.GetComponentInParent<Door>();


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
            if (isSelected&&!door.isend)
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
        //포지션 업데이트 안되게
        if (!ispos)
        {
            attachpos = interactor.GetAttachTransform(this).position;
            ispos = true;
        }
        ydis = interactor.GetAttachTransform(this).position.y - attachpos.y;
        Debug.Log(ydis);
        if (ydis != 0)
        {
            rotateobj.transform.Rotate(0, 0, -ydis);
            door.UpDoor(1.5f);
        }
        if (door.feDoor.transform.position.y > door.fedoorpos.y + pos)
        {
      
            door.enddoor();
            transform.GetComponent<DoorGrab>().enabled = false;
        }
    }

}
