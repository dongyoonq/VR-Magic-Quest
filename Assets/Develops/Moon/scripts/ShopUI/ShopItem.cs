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
   // [SerializeField] public RuneItem runetiem;
    public void PopUI()
    {

      shuopui= Instantiate(intateUI, Camera.main.transform.position+Camera.main.transform.forward.normalized, Quaternion.identity);
        shuopui.shopItem = this;


        Debug.Log("����");
    
    }
    public void Buy()
    {
        Debug.Log("������������ ��");
        player.AddItemToInventory(itemData);
        Destroy(shuopui.gameObject);
        Destroy(gameObject);
    }
    public void Cancel()
    {
        Destroy(shuopui.gameObject);
    }
}
