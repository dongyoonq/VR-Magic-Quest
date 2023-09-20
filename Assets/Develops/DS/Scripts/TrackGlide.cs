using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGlide : MonoBehaviour
{
    private Track track;
    float time;

    private void Awake()
    {
        track = GetComponentInParent<Track>();
        time = 1f;
    }

    private void OnTriggerStay(Collider other)
    {
        time += Time.deltaTime;
        if (track.TrainWheelHolder != null)
        {
            track.TrainWheelHolder.Glide(transform.position);
        }
    }
}
