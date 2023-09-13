using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 소비 아이템 정보 </summary>
[CreateAssetMenu(fileName = "Item_Portion_", menuName = "Inventory System/Item Data/Portion", order = 5)]
public class PortionItemData : CountableItemData
{
    /// <summary> 효과량(회복량 등) </summary>
    public float Value => value;
    [SerializeField] private float value;

    public override bool UseItem()
    {
        Vector3 createVector = Camera.main.transform.position;
        createVector += Camera.main.transform.forward * 1.5f;

        Instantiate<GameObject>(dropItemPrefab, createVector, Camera.main.transform.rotation);
        return true;
    }
}
