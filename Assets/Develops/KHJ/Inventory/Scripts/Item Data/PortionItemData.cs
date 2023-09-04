using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 소비 아이템 정보 </summary>
[CreateAssetMenu(fileName = "Item_Portion_", menuName = "Inventory System/Item Data/Portion", order = 3)]
public class PortionItemData : CountableItemData
{
    /// <summary> 효과량(회복량 등) </summary>
    public float Value => _value;
    [SerializeField] private float _value;
    public override Item CreateItem()
    {
        return new PortionItem(this);
    }

    public override void UseItem()
    {
        base.UseItem();

        Vector3 createVector = Camera.main.transform.position;
        createVector += Camera.main.transform.forward * 1.5f;

        Instantiate<GameObject>(_dropItemPrefab, createVector, Camera.main.transform.rotation);
    }
}
