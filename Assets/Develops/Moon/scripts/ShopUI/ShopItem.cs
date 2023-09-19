using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ShopItem : MonoBehaviour
{

    [SerializeField] public ShopUI intateUI;

    [SerializeField] public ItemData itemData;
    public Player player;
    [SerializeField] public ShopUI shuopui;

    [SerializeField] public Shop shop;
    public void Awake()
    {
        player = GameObject.FindObjectOfType<Player>();
        shop = GetComponentInParent<Shop>();
    }

    public void PopUI()
    {
        if (shop.isOpenShopItem)
        {
            Destroy(shop.shopUI.gameObject);
            shop.isOpenShopItem = false;
        }

        shuopui = Instantiate(intateUI, Camera.main.transform.position + Camera.main.transform.forward.normalized, Quaternion.identity);
        shuopui.transform.forward = Camera.main.transform.forward;
        shuopui.shopItem = this;

        shop.isOpenShopItem = true;
        shop.shopUI = shuopui;
      
    
    }
    public void Buy()
    {
     
        player.AddItemToInventory(itemData);
        shop.isOpenShopItem = false;
        Destroy(shuopui.gameObject);
        Destroy(gameObject);
        GameManager.Sound.PlaySFX("shopbuysound");
 
    }
    public void Cancel()
    {
        Destroy(shuopui.gameObject);
        shop.isOpenShopItem = false;
    }
}
