using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Button button;

    public ItemData data;
    public int slotIndex;
    public int amount = 0;

    public void UseItem()
    {
        if (data != null)
        {
            Player player = FindAnyObjectByType<Player>();
            CountableItemData countData = data as CountableItemData;
            countData.UseItem();
            player.RemoveItemFromInventory(data, slotIndex);
        }
    }
}