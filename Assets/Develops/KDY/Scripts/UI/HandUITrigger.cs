using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandUITrigger : MonoBehaviour
{
    [SerializeField] BookCanvasUI bookCanvasUI;

    bool isActive = false;
    bool isReady = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Hand") && isReady)
        {
            isActive = !isActive;
            bookCanvasUI.ActiveBookUI(isActive);

            isReady = false;
            StartCoroutine(ReadyRoutine());
        }
    }

    public void CloseBook()
    {
        isActive = false;
        bookCanvasUI.ActiveBookUI(isActive);
    }

    IEnumerator ReadyRoutine()
    {
        yield return new WaitForSeconds(1f);
        isReady = true;
    }
}
