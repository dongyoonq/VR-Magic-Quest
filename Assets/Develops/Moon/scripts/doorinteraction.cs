using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class doorinteraction : MonoBehaviour
{
    [SerializeField] public GameObject rightcontroller;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            Debug.Log("진입");
            rightcontroller.GetComponent<SphereCollider>().enabled = true;
        }
        

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            Debug.Log("나감");
            rightcontroller.GetComponent<SphereCollider>().enabled = false;
        }
           
    }
}
