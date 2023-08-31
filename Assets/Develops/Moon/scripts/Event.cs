using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Event : MonoBehaviour
{
   [SerializeField] public SelectEnterEvent OnSelectedEnter;

    void Awake()
    {
        var interactor = GetComponent<XRBaseInteractor>();
        interactor.selectEntered.AddListener(evt => { OnSelectedEnter.Invoke(evt); });
    }
}
