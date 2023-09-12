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
            Debug.Log("´êÀ½");
        }
    }

    public void PopUI()
    {
        GameObject objUI = Instantiate(intateUI);
        objUI.transform.position = Camera.main.transform.forward;
        Debug.Log("´êÀ½");
    }
    public void Buy()
    {
        gameObject.AddComponent<XRGrabInteractable>();
        gameObject.GetComponent<XRGrabInteractable>().attachTransform = Attachpoint;
        
        //TODO: ÀÎº¥Åä¸® ºó°ø°£ µé¾î°¡°Ô²û ¸¸µé±â
        Destroy(gameObject.GetComponent<XRSimpleInteractable>());
        Destroy(gameObject);
    }
}
