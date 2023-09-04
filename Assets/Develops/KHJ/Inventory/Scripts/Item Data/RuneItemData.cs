using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 소비 아이템 정보 </summary>
[CreateAssetMenu(fileName = "Item_Rune", menuName = "Inventory System/Item Data/Rune", order = 4)]
public class RuneItemData : CountableItemData
{


    public override Item CreateItem()
    {
        return new RuneItem(this);
    }

    public override void UseItem()
    {
        base.UseItem();

        Vector3 createVector = Camera.main.transform.position;
        createVector += Camera.main.transform.forward * 1.5f;

        Instantiate<GameObject>(_dropItemPrefab, createVector, Camera.main.transform.rotation);
    }
}
