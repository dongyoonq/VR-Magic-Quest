using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler
{
    public Button button;

    public ItemData data;
    public int slotIndex;
    public int amount = 0;

    private InventoryUI inventoryUI;

    private void Start()
    {
        inventoryUI = GetComponentInParent<InventoryUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (inventoryUI == null || inventoryUI.inventoryInfoUI == null || data == null)
            return;

        inventoryUI.inventoryInfoUI.itemName.text = data.Name;
        inventoryUI.inventoryInfoUI.itemDescription.text = data.Tooltip;
    }

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