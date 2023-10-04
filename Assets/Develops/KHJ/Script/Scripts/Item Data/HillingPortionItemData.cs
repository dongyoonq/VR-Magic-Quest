using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_Portion_", menuName = "Inventory System/Item Data/HillingPortion", order = 7)]
public class HillingPortionItemData : PortionItemData
{
    public int HillHpValue => hillHpValue;
    public int HillMpValue => hillMpValue;
    [SerializeField] private int hillHpValue;
    [SerializeField] private int hillMpValue;
}
