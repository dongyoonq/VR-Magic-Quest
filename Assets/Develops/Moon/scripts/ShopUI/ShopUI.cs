using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{

    public ShopItem shopItem;
   public void Buy()
    {
        shopItem.Buy();
    }

    public void Cancel()
    {
        shopItem.Cancel();
    //    Destroy(gameObject);
    }
}
