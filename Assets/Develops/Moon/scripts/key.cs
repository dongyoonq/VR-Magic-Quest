using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit;

public class key : MonoBehaviour
{
    [SerializeField] public string keytype;

    public void Use()
    {
        Destroy(gameObject);
    }
}
