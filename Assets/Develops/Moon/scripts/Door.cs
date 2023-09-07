using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject feDoor;
    public bool interaction;
    [SerializeField] Quaternion angle;

    public void Update()
    {
        angle = transform.rotation;
    }
    public void UpDoor()
    {
        interaction = true;
        StartCoroutine(uproutin());
    }
    public void DownDoor()
    {
        interaction = false;
        StartCoroutine(downroutin());
    }

    IEnumerator uproutin()
    {
        while (interaction)
        {
            feDoor.GetComponent<Rigidbody>().velocity = new Vector3(0, 3, 0);
            yield return null;
        }
    }
    IEnumerator downroutin()
    {
        while (angle.z>0)
        {
            transform.Rotate(0, 0, -30 * Time.deltaTime);
            yield return null;
        }
    }

}
