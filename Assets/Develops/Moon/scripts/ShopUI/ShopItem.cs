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

    public void PopUI()
    {

      shuopui= Instantiate(intateUI, Camera.main.transform.position+Camera.main.transform.forward.normalized*0.5f, Quaternion.identity);
        shuopui.shopItem = this;
        Debug.Log("닿음");
    
    }
    public void Buy()
    {
        Debug.Log("샵아이템으로 들어감");
        player.AddItemToInventory(itemData);
        Destroy(shuopui.gameObject);
        Destroy(gameObject);
    }
    public void Cancel()
    {
        Destroy(shuopui.gameObject);
    }
}
