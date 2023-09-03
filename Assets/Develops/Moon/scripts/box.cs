using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class box : MonoBehaviour
{
    [SerializeField] public GameObject openobj;


    public void Open()
    {
        openobj.transform.Rotate(-50f, 0, 0);
    }
}
