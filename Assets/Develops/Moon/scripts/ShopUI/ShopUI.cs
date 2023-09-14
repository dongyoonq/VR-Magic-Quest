using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
   // [SerializeField] public Button buybutton;
  //  [SerializeField] public Button cancelbutton;

    public ShopItem shopItem;
   public void Buy()
    {
        shopItem.Buy();
    }

    public void Cancel()
    {
        Destroy(gameObject);
    }
}
