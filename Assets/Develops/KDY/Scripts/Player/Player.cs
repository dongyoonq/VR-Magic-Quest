using PDollarGestureRecognizer;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public UnityEvent<ItemData, int, int> OnAddItemInventory;
    public UnityEvent<ItemData, int, int> OnRemoveItemInventory;

    public InventoryUI inventoryUI { get; set; }
    public Inventory inventory;

    public List<SkillData> skillList;
    public List<PortionRecipeData> unlockRecipeList;
    public List<Gesture> trainingSet = new List<Gesture>();

    [SerializeField] public int maxHp;
    [SerializeField] public int maxMp;
    public int currHp;
    public int currMp;

    [NonSerialized] public bool isSkillUsed;

    private void Start()
    {
        currHp = maxHp;
        currMp = maxMp;

        trainingSet = GameManager.Load.LoadGestures();
    }


    // �κ��丮 �߰� �޼���
    public void AddItemToInventory(ItemData item)
    {
        if (inventory.list.Count(x => x == null) == 0)
        {
            Debug.Log("������ ����á��");
            return;
        }

        int index = -1;
        if (item is CountableItemData)
        {
            index = inventory.list.FindIndex(x => x == item);

            if (index == -1)
            {
                for (int i = 0; i < inventory.list.Count; i++)
                {
                    if (inventory.list[i] == null)
                    {
                        inventory.list[i] = item;
                        inventoryUI.slots[i].slotIndex = i;
                        index = i;
                        break;
                    }
                }
            }

            OnAddItemInventory?.Invoke(item, index, 1);
            return;
        }

        for (int i = 0; i < inventory.list.Count; i++)
        {
            if (inventory.list[i] == null)
            {
                inventory.list[i] = item;
                inventoryUI.slots[i].slotIndex = i;
                index = i;
                break;
            }
        }

        OnAddItemInventory?.Invoke(item, index, 1);
    }

    public void AddItemToInventory(ItemData item, int index = 0)
    {
        OnAddItemInventory?.Invoke(item, index, 1);
    }

    // �κ��丮 ������ ���� �޼���
    public void RemoveItemFromInventory(ItemData item, int index = -1)
    {
        if (index == -1)
        {
            for (int i = 0; i < inventory.list.Count; i++)
            {
                if (inventory.list[i] == item)
                {
                    index = i;
                    break;
                }
            }
        }

        OnRemoveItemInventory?.Invoke(item, index, 1);

        if (item is not CountableItemData)
        {
            inventory.list[index] = null;
        }
        else if (item is CountableItemData)
        {
            if (inventoryUI.slots[index].amount == 0)
                inventory.list[index] = null;
        }
    }

    public void UnlockRecipe(PortionRecipeData data)
    {
        if (!unlockRecipeList.Contains(data))
            unlockRecipeList.Add(data);
    }

    public void LockRecipe(PortionRecipeData data)
    {
        if (unlockRecipeList.Contains(data))
            unlockRecipeList.Remove(data);
    }
}