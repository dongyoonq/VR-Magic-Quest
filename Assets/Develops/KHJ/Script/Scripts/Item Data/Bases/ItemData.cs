using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    [상속 구조]

    ItemData(abstract)
        - CountableItemData(abstract)
            - PortionItemData
        - EquipmentItemData(abstract)
            - WeaponItemData
            - ArmorItemData

*/

public abstract class ItemData : ScriptableObject
{
    public int ID => id;
    public string Name => itemName;
    public string Tooltip => tooltip;
    public Sprite IconSprite => iconSprite;
    public GameObject DropItemPrefab => dropItemPrefab;

    [SerializeField] private int id;
    [SerializeField] private string itemName;    // 아이템 이름
    [Multiline]
    [SerializeField] private string tooltip; // 아이템 설명
    [SerializeField] private Sprite iconSprite; // 아이템 아이콘
    [SerializeField] protected GameObject dropItemPrefab; // 바닥에 떨어질 때 생성할 프리팹
}