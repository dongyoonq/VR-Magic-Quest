using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    Inventory inventory;

    public RectTransform inventoryPanel;
    public RectTransform contents;
    public InventoryInfoUI inventoryInfoUI;

    public InventorySlot[] slots;

    private Player player;

    private void Start()
    {
        if (contents != null)
            slots = contents.GetComponentsInChildren<InventorySlot>();
    }

    private void OnEnable()
    {
        player = FindAnyObjectByType<Player>();

        inventory = player.inventory;
        player.inventoryUI = this;
        inventory.onSlotCountChange.AddListener(SlotChange);

        for (int i = 0; i < slots.Length; i++)
            slots[i].slotIndex = i;
    }

    private void SlotChange(int val)
    {

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.SlotCnt)
                slots[i].GetComponent<Button>().interactable = true;
            else
                slots[i].GetComponent<Button>().interactable = false;
        }
    }

    public void AddSlot()
    {
        inventory.SlotCnt++;
    }
}