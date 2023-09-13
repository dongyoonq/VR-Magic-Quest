using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class ShopItem : MonoBehaviour
{

    [SerializeField] GameObject intateUI;
    [SerializeField] Transform Attachpoint;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 7)
        {
            Debug.Log("����");
        }
    }

    public void PopUI()
    {
        GameObject objUI = Instantiate(intateUI);
        objUI.transform.position = Camera.main.transform.forward;
        Debug.Log("����");
    }
    public void Buy()
    {
        gameObject.AddComponent<XRGrabInteractable>();
        gameObject.GetComponent<XRGrabInteractable>().attachTransform = Attachpoint;
        
        //TODO: �κ��丮 ����� ���Բ� �����
        Destroy(gameObject.GetComponent<XRSimpleInteractable>());
        Destroy(gameObject);
    }
}
