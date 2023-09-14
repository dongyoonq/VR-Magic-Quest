using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class ShopItem : MonoBehaviour
{

    [SerializeField] ShopUI intateUI;
    [SerializeField] Transform Attachpoint;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 7)
        {
          
            PopUI();
        }
    }

    public void PopUI()
    {   
       ShopUI shopui=Instantiate(intateUI, Camera.main.transform.position + Camera.main.transform.forward,Quaternion.identity);
        Debug.Log("����");
    }
    public void Buy()
    {

        Debug.Log("������������ ��");
        gameObject.AddComponent<XRGrabInteractable>();
        gameObject.GetComponent<XRGrabInteractable>().attachTransform = Attachpoint;
        
        //TODO: �κ��丮 ����� ���Բ� �����
        Destroy(gameObject.GetComponent<XRSimpleInteractable>());
        Destroy(gameObject);
    }
}
