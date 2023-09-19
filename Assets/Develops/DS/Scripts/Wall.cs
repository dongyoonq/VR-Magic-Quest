using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField]
    private GameObject particle;

    private void Awake()
    {
        particle = GameManager.Resource.Instantiate(particle, transform.position, transform.rotation);
        particle.transform.localScale = transform.localScale / 10f;
        particle.SetActive(false);
    }

    public void Unlock()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        particle.SetActive(false);
        particle.SetActive(true);
        particle.transform.position = other.transform.position + other.transform.up * 1.1f + other.transform.forward * 0.7f;
    }
}
