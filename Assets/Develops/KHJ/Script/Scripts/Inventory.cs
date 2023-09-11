using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public UnityEvent<int> onSlotCountChange;
    public UnityEvent onChangeInventory;

    private int slotCnt;

    private Player player;

    public int SlotCnt
    {
        get => slotCnt;
        set { slotCnt = value; onSlotCountChange?.Invoke(slotCnt); }

    }

    /////////////////////////////////////////////////////

    public List<ItemData> list;

    private void Start()
    {
        player = FindAnyObjectByType<Player>();

        player.OnAddItemInventory.AddListener(AddInventory);
        player.OnRemoveItemInventory.AddListener(RemoveInventory);

        SlotCnt = 28;
        list = new List<ItemData>(SlotCnt);
        for (int i = 0; i < SlotCnt; i++)
            list.Add(null);
    }

    void AddInventory(ItemData itemData, int index, int amount)
    {
        if (itemData is CountableItemData)
            player.inventoryUI.slots[index].amount++;
        else
            player.inventoryUI.slots[index].amount = 1;

        player.inventoryUI.slots[index].GetComponent<Button>().onClick.RemoveAllListeners();
        player.inventoryUI.slots[index].GetComponent<Button>().onClick.AddListener(() => player.inventoryUI.slots[index].UseItem());
        player.inventoryUI.slots[index].transform.GetChild(0).gameObject.SetActive(true);
        player.inventoryUI.slots[index].transform.GetChild(0).GetComponent<Image>().sprite = itemData.IconSprite;
        player.inventoryUI.slots[index].data = itemData;

        player.inventoryUI.slots[index].transform.GetChild(1).GetComponent<TMP_Text>().text = $"{player.inventoryUI.slots[index].amount}";
    }

    void RemoveInventory(ItemData itemData, int index, int amount)
    {
        if (itemData is CountableItemData)
        {
            player.inventoryUI.slots[index].amount--;

            if (player.inventoryUI.slots[index].amount == 0)
            {
                player.inventoryUI.slots[index].transform.GetChild(0).GetComponent<Image>().sprite = null;
                player.inventoryUI.slots[index].transform.GetChild(0).gameObject.SetActive(false);

                player.inventoryUI.slots[index].data = null;
            }
        }
        else
        {
            player.inventoryUI.slots[index].amount = 0;

            player.inventoryUI.slots[index].transform.GetChild(0).GetComponent<Image>().sprite = null;
            player.inventoryUI.slots[index].transform.GetChild(0).gameObject.SetActive(false);

            player.inventoryUI.slots[index].data = null;
        }

        player.inventoryUI.slots[index].transform.GetChild(1).GetComponent<TMP_Text>().text = $"{player.inventoryUI.slots[index].amount}";
    }
}