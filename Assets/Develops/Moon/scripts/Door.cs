using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject feDoor;
    public bool interaction;
    public void UpDoor()
    {
        interaction = true;
        StartCoroutine(uproutin());
    }
    public void DownDoor()
    {
        interaction = false;
    }

    IEnumerator uproutin()
    {
        while (interaction)
        {
            feDoor.GetComponent<Rigidbody>().velocity = new Vector3(0, 3, 0);
            yield return null;
        }
    }

}
