using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit;

public class key : MonoBehaviour
{
    [SerializeField] public string keytype;
    public SelectEnterEvent selectEnterEvent;

    public void Awake()
    {
        var interatable = GetComponent<XRBaseInteractable>();
        interatable.selectEntered.AddListener(selectedSwitch);
    }
    public void Use()
    {
        Destroy(gameObject, 1f);
    }

    public void selectedSwitch(SelectEnterEventArgs args)
    {
        var interator = args.interactor;
        var socketinterator = interator as boxInteraction;

        if (socketinterator == null)
            return;
        if (keytype != socketinterator.keytype)
            return;

        selectEnterEvent.Invoke(args);
    }

}
