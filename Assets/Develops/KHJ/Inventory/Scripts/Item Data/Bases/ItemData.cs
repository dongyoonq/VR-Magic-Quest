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
    [SerializeField]
    private Iteminfo[] iteminfos;
    public Iteminfo[] Iteminfos { get { return iteminfos; } }
    [Serializable]
    public class Iteminfo
    {
        public int _id;
        public string _name;    // 아이템 이름
        public string _tooltip; // 아이템 설명
        public Sprite _iconSprite; // 아이템 아이콘
        public GameObject _dropItemPrefab; // 바닥에 떨어질 때 생성할 프리팹
    }


    /// <summary> 타입에 맞는 새로운 아이템 생성 </summary>
    public abstract Item CreateItem();
    public virtual void UseItem() {}
}