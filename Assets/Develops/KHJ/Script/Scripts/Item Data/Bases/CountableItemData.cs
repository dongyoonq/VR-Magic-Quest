using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 셀 수 있는 아이템 데이터 </summary>
public abstract class CountableItemData : ItemData
{
    /// <summary> 아이템 최대 개수 </summary>
    public int MaxAmount => maxAmount;
    [SerializeField] private int maxAmount = 99;

    /// <summary> 현재 아이템 개수 </summary>
    public int Amount { get; protected set; }

    /// <summary> 수량이 가득 찼는지 여부 </summary>
    public bool IsMax => Amount >= MaxAmount;

    /// <summary> 개수 지정(범위 제한) </summary>
    public void SetAmount(int amount)
    {
        Amount = Mathf.Clamp(amount, 0, MaxAmount);
    }
        
    public abstract bool UseItem();

    /// <summary> 개수 추가 및 최대치 초과량 반환(초과량 없을 경우 0) </summary>
    public int AddAmountAndGetExcess(int amount)
    {
        int nextAmount = Amount + amount;
        SetAmount(nextAmount);

        return (nextAmount > MaxAmount) ? (nextAmount - MaxAmount) : 0;
    }
}