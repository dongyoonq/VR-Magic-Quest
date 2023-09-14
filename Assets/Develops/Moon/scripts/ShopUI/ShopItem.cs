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
        Debug.Log("닿음");
    }
    public void Buy()
    {

        Debug.Log("샵아이템으로 들어감");
        gameObject.AddComponent<XRGrabInteractable>();
        gameObject.GetComponent<XRGrabInteractable>().attachTransform = Attachpoint;
        
        //TODO: 인벤토리 빈공간 들어가게끔 만들기
        Destroy(gameObject.GetComponent<XRSimpleInteractable>());
        Destroy(gameObject);
    }
}
