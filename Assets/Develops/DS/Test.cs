using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    private void Start()
    {
        GameObject a = GameManager.Resource.Instantiate(prefab, transform.position, Quaternion.identity, true);
        StartCoroutine(TestRoutine(a));
    }

    private IEnumerator TestRoutine(GameObject a)
    {
        GameManager.Resource.Instantiate(prefab, transform.position, Quaternion.identity, true);
        yield return new WaitForSeconds(3);
        GameManager.Resource.Destroy(a);
        yield return new WaitForSeconds(3);
        GameManager.Resource.Instantiate(prefab, transform.position, Quaternion.identity, true);
    }
}
