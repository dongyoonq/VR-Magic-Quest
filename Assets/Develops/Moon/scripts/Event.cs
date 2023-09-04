using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class Event : MonoBehaviour
{
    public boxInteraction boxInteraction;
    public UnityEvent envet;
    void Awake()
    {
        boxInteraction.selectEntered.AddListener((evt) => envet.Invoke());
    }
}
 


