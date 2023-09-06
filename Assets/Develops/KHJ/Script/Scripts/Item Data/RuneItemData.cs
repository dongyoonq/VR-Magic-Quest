using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 소비 아이템 정보 </summary>
[CreateAssetMenu(fileName = "Item_Rune", menuName = "Inventory System/Item Data/Rune", order = 4)]
public class RuneItemData : CountableItemData
{
    public override bool UseItem()
    {
        if (Amount <= 0)
        {
            return false;
        }
        else
        {
            Amount -= 1;
            Vector3 createVector = Camera.main.transform.position;
            createVector += Camera.main.transform.forward * 1.5f;

            Instantiate<GameObject>(dropItemPrefab, createVector, Camera.main.transform.rotation);
            return true;
        }
    }
}
