using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Questpaper : MonoBehaviour
{
    public Rigidbody rb;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
}
