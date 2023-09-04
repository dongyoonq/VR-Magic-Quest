using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 수량 아이템 - 룬 아이템 </summary>
public class RuneItem : CountableItem, IUsableItem
{
    public RuneItem(RuneItemData data, int amount = 1) : base(data, amount) { }

    public bool Use()
    {
        Data.UseItem();

        Amount--;

        return true;
    }
}