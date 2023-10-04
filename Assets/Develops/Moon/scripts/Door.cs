using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Door : MonoBehaviour
{
    [SerializeField] public GameObject feDoor;
    public bool interaction;
    [SerializeField] Quaternion angle;
    [SerializeField] public float sped;
    [SerializeField] public bool isend = false;
    [SerializeField] public Vector3 fedoorpos;
    public void Awake()
    {
        fedoorpos = feDoor.transform.position;
    }
    public void Update()
    {
        angle = transform.rotation;
    }
    public void UpDoor(float sped)
    {
     //   interaction = true;
     //   StartCoroutine(uproutin(sped));
         if (!isend)
          {
              interaction = true;
              StartCoroutine(uproutin(sped));
          }
          else
          {
              enddoor();
          }
    }
    public void DownDoor()
    {
        interaction = false;
        StartCoroutine(downroutin());
        if (!isend)
        {
            interaction = false;
            StartCoroutine(downroutin());
        }
       else
        {
            enddoor();
        }
    }

    IEnumerator uproutin(float sped)
    {
        if (!isend)
        {
            while (interaction)
            {
                feDoor.GetComponent<Rigidbody>().velocity = new Vector3(0, sped, 0);
                yield return null;
            }
        }
           
    }
    IEnumerator downroutin()
    {
        if (!isend)
        {
           while (angle.z < 0)
               {
                   transform.Rotate(0, 0, 30 * Time.deltaTime);
                   yield return null;
               }

        }

    }
    public void enddoor()
    {
       
        isend = true;
        feDoor.GetComponent<Rigidbody>().useGravity = false;
        feDoor.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

}
