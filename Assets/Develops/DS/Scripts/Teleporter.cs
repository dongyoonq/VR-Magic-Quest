using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    private GameObject[] teleportationAnchorsGroup;

    private void Awake()
    {
        foreach (GameObject teleportationAnchor in teleportationAnchorsGroup)
        {
            teleportationAnchor.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (GameObject teleportationAnchor in teleportationAnchorsGroup)
        {
            teleportationAnchor.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (GameObject teleportationAnchor in teleportationAnchorsGroup)
        {
            teleportationAnchor.SetActive(false);
        }
    }
}
