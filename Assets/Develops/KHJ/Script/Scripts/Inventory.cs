using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    [Item의 상속구조]
    - Item
        - CountableItem
            - PortionItem : IUsableItem.Use() -> 사용 및 수량 1 소모
        - EquipmentItem
            - WeaponItem
            - ArmorItem

    [ItemData의 상속구조]
      (ItemData는 해당 아이템이 공통으로 가질 데이터 필드 모음)
      (개체마다 달라져야 하는 현재 내구도, 강화도 등은 Item 클래스에서 관리)

    - ItemData
        - CountableItemData
            - PortionItemData : 효과량(Value : 회복량, 공격력 등에 사용)
        - EquipmentItemData : 최대 내구도
            - WeaponItemData : 기본 공격력
            - ArmorItemData : 기본 방어력
*/

/*
    [API]
    - bool HasItem(int) : 해당 인덱스의 슬롯에 아이템이 존재하는지 여부
    - bool IsCountableItem(int) : 해당 인덱스의 아이템이 셀 수 있는 아이템인지 여부
    - int GetCurrentAmount(int) : 해당 인덱스의 아이템 수량
        - -1 : 잘못된 인덱스
        -  0 : 빈 슬롯
        -  1 : 셀 수 없는 아이템이거나 수량 1
    - ItemData GetItemData(int) : 해당 인덱스의 아이템 정보
    - string GetItemName(int) : 해당 인덱스의 아이템 이름

    - int Add(ItemData, int) : 해당 타입의 아이템을 지정한 개수만큼 인벤토리에 추가
        - 자리 부족으로 못넣은 개수만큼 리턴(0이면 모두 추가 성공했다는 의미)
    - void Remove(int) : 해당 인덱스의 슬롯에 있는 아이템 제거
    - void Swap(int, int) : 두 인덱스의 아이템 위치 서로 바꾸기
    - void SeparateAmount(int a, int b, int amount)
        - a 인덱스의 아이템이 셀 수 있는 아이템일 경우, amount만큼 분리하여 b 인덱스로 복제
    - void Use(int) : 해당 인덱스의 아이템 사용
    - void UpdateSlot(int) : 해당 인덱스의 슬롯 상태 및 UI 갱신
    - void UpdateAllSlot() : 모든 슬롯 상태 및 UI 갱신
    - void UpdateAccessibleStatesAll() : 모든 슬롯 UI에 접근 가능 여부 갱신
    - void TrimAll() : 앞에서부터 아이템 슬롯 채우기
    - void SortAll() : 앞에서부터 아이템 슬롯 채우면서 정렬
*/

public class Inventory : MonoBehaviour
{
    /// <summary> 아이템 수용 한도 </summary>
    public int Capacity { get; private set; }

    // /// <summary> 현재 아이템 개수 </summary>
    //public int ItemCount => _itemArray.Count;

    // 초기 수용 한도
    [SerializeField, Range(8, 64)]
    private int initalCapacity = 64;

    // 최대 수용 한도(아이템 배열 크기)
    [SerializeField, Range(8, 64)]
    private int maxCapacity = 64;

    [SerializeField]
    private InventoryUI inventoryUI; // 연결된 인벤토리 UI

    /// <summary> 아이템 목록 </summary>
    [SerializeField]
    private ItemData[] items;

    /// <summary> 업데이트 할 인덱스 목록 </summary>
    private readonly HashSet<int> indexSetForUpdate = new HashSet<int>();

    /// <summary> 아이템 데이터 타입별 정렬 가중치 </summary>
    private readonly static Dictionary<Type, int> sortWeightDict = new Dictionary<Type, int>
    {
        { typeof(PortionItemData), 10000 },
        /*{ typeof(WeaponItemData),  20000 },
        { typeof(ArmorItemData),   30000 },*/
    };

    private class ItemComparer : IComparer<ItemData>
    {
        public int Compare(ItemData a, ItemData b)
        {
            return (a.ID + sortWeightDict[a.GetType()])
                 - (b.ID + sortWeightDict[b.GetType()]);
        }
    }
    private static readonly ItemComparer itemComparer = new ItemComparer();


    #if UNITY_EDITOR
    private void OnValidate()
    {
        if(initalCapacity > maxCapacity)
            initalCapacity = maxCapacity;
    }
    #endif
    private void Awake()
    {
        items = new ItemData[maxCapacity];
        Capacity = initalCapacity;
        inventoryUI.SetInventoryReference(this);
    }

    private void Start()
    {
        UpdateAccessibleStatesAll();
    }

    /// <summary> 인덱스가 수용 범위 내에 있는지 검사 </summary>
    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < Capacity;
    }

    /// <summary> 앞에서부터 비어있는 슬롯 인덱스 탐색 </summary>
    private int FindEmptySlotIndex(int startIndex = 0)
    {
        for (int i = startIndex; i < Capacity; i++)
            if (items[i] == null)
                return i;
        return -1;
    }

    /// <summary> 앞에서부터 개수 여유가 있는 Countable 아이템의 슬롯 인덱스 탐색 </summary>
    private int FindCountableItemSlotIndex(CountableItemData target, int startIndex = 0)
    {
        for (int i = startIndex; i < Capacity; i++)
        {
            var current = items[i];
            if (current == null)
                continue;

            // 아이템 종류 일치, 개수 여유 확인
            if (current == target && current is CountableItemData ci)
            {
                if (!ci.IsMax)
                    return i;
            }
        }

        return -1;
    }

    /// <summary> 해당하는 인덱스의 슬롯 상태 및 UI 갱신 </summary>
    private void UpdateSlot(int index)
    {
        if (!IsValidIndex(index)) return;

        ItemData item = items[index];

        // 1. 아이템이 슬롯에 존재하는 경우
        if (item != null)
        {
            // 아이콘 등록
            inventoryUI.SetItemIcon(index, item.IconSprite);

            // 1-1. 셀 수 있는 아이템
            if (item is CountableItemData ci)
            {
                // 1-1-1. 수량이 0인 경우, 아이템 제거
                if (ci.IsEmpty)
                {
                    items[index] = null;
                    RemoveIcon();
                    return;
                }
                // 1-1-2. 수량 텍스트 표시
                else
                {
                    inventoryUI.SetItemAmountText(index, ci.Amount);
                }
            }
            // 1-2. 셀 수 없는 아이템인 경우 수량 텍스트 제거
            else
            {
                inventoryUI.HideItemAmountText(index);
            }

            // 슬롯 필터 상태 업데이트
            inventoryUI.UpdateSlotFilterState(index, item);
        }
        // 2. 빈 슬롯인 경우 : 아이콘 제거
        else
        {
            RemoveIcon();
        }

        // 로컬 : 아이콘 제거하기
        void RemoveIcon()
        {
            inventoryUI.RemoveItem(index);
            inventoryUI.HideItemAmountText(index); // 수량 텍스트 숨기기
        }
    }

    /// <summary> 해당하는 인덱스의 슬롯들의 상태 및 UI 갱신 </summary>
    private void UpdateSlot(params int[] indices)
    {
        foreach (var i in indices)
        {
            UpdateSlot(i);
        }
    }

    /// <summary> 모든 슬롯들의 상태를 UI에 갱신 </summary>
    private void UpdateAllSlot()
    {
        for (int i = 0; i < Capacity; i++)
        {
            UpdateSlot(i);
        }
    }


    /// <summary> 해당 슬롯이 아이템을 갖고 있는지 여부 </summary>
    public bool HasItem(int index)
    {
        return IsValidIndex(index) && items[index] != null;
    }

    /// <summary> 해당 슬롯이 셀 수 있는 아이템인지 여부 </summary>
    public bool IsCountableItem(int index)
    {
        return HasItem(index) && items[index] is CountableItemData;
    }

    /// <summary> 
    /// 해당 슬롯의 현재 아이템 개수 리턴
    /// <para/> - 잘못된 인덱스 : -1 리턴
    /// <para/> - 빈 슬롯 : 0 리턴
    /// <para/> - 셀 수 없는 아이템 : 1 리턴
    /// </summary>
    public int GetCurrentAmount(int index)
    {
        if (!IsValidIndex(index)) return -1;
        if (items[index] == null) return 0;

        CountableItemData ci = items[index] as CountableItemData;
        if (ci == null)
            return 1;

        return ci.Amount;
    }

    /// <summary> 해당 슬롯의 아이템 정보 리턴 </summary>
    public ItemData GetItemData(int index)
    {
        if (!IsValidIndex(index)) return null;
        if (items[index] == null) return null;

        return items[index];
    }

    /// <summary> 해당 슬롯의 아이템 이름 리턴 </summary>
    public string GetItemName(int index)
    {
        if (!IsValidIndex(index)) return "";
        if (items[index] == null) return "";

        return items[index].Name;
    }

    /// <summary> 인벤토리 UI 연결 </summary>
    public void ConnectUI(InventoryUI inventoryUI)
    {
        this.inventoryUI = inventoryUI;
        this.inventoryUI.SetInventoryReference(this);
    }

    /// <summary> 인벤토리에 아이템 추가
    /// <para/> 넣는 데 실패한 잉여 아이템 개수 리턴
    /// <para/> 리턴이 0이면 넣는데 모두 성공했다는 의미
    /// </summary>
    public int Add(ItemData itemData, int amount = 1)
    {
        int index;

        // 1. 수량이 있는 아이템
        if (itemData is CountableItemData ciData)
        {
            bool findNextCountable = true;
            index = -1;

            while (amount > 0)
            {
                // 1-1. 이미 해당 아이템이 인벤토리 내에 존재하고, 개수 여유 있는지 검사
                if (findNextCountable)
                {
                    index = FindCountableItemSlotIndex(ciData, index + 1);

                    // 개수 여유있는 기존재 슬롯이 더이상 없다고 판단될 경우, 빈 슬롯부터 탐색 시작
                    if (index == -1)
                    {
                        findNextCountable = false;
                    }
                    // 기존재 슬롯을 찾은 경우, 양 증가시키고 초과량 존재 시 amount에 초기화
                    else
                    {
                        CountableItemData ci = items[index] as CountableItemData;
                        amount = ci.AddAmountAndGetExcess(amount);

                        UpdateSlot(index);
                    }
                }
                // 1-2. 빈 슬롯 탐색
                else
                {
                    index = FindEmptySlotIndex(index + 1);

                    // 빈 슬롯조차 없는 경우 종료
                    if (index == -1)
                    {
                        break;
                    }
                    // 빈 슬롯 발견 시, 슬롯에 아이템 추가 및 잉여량 계산
                    else
                    {
                        // 새로운 아이템 생성
                        CountableItemData ci = ciData as CountableItemData;
                        ci.SetAmount(amount);

                        // 슬롯에 추가
                        items[index] = ci;

                        // 남은 개수 계산
                        amount = (amount > ciData.MaxAmount) ? (amount - ciData.MaxAmount) : 0;

                        UpdateSlot(index);
                    }
                }
            }
        }
        // 2. 수량이 없는 아이템
        else
        {
            // 2-1. 1개만 넣는 경우, 간단히 수행
            if (amount == 1)
            {
                index = FindEmptySlotIndex();
                if (index != -1)
                {
                    // 아이템을 생성하여 슬롯에 추가
                    items[index] = itemData;
                    amount = 0;

                    UpdateSlot(index);
                }
            }

            // 2-2. 2개 이상의 수량 없는 아이템을 동시에 추가하는 경우
            index = -1;
            for (; amount > 0; amount--)
            {
                // 아이템 넣은 인덱스의 다음 인덱스부터 슬롯 탐색
                index = FindEmptySlotIndex(index + 1);

                // 다 넣지 못한 경우 루프 종료
                if (index == -1)
                {
                    break;
                }

                // 아이템을 생성하여 슬롯에 추가
                items[index] = itemData;

                UpdateSlot(index);
            }
        }

        return amount;
    }

    /// <summary> 해당 슬롯의 아이템 제거 </summary>
    public void Remove(int index)
    {
        if (!IsValidIndex(index)) return;

        items[index] = null;
        inventoryUI.RemoveItem(index);
    }

    /// <summary> 해당 슬롯의 아이템 사용 </summary>
    public void Use(int index)
    {
        if (!IsValidIndex(index)) return;
        if (items[index] == null) return;

        // 사용 가능한 아이템인 경우
        if (items[index] is CountableItemData uItem)
        {
            // 아이템 사용
            bool succeeded = uItem.UseItem();

            if (succeeded)
            {
                UpdateSlot(index);
            }
        }
    }

    /// <summary> 모든 슬롯 UI에 접근 가능 여부 업데이트 </summary>
    public void UpdateAccessibleStatesAll()
    {
        inventoryUI.SetAccessibleSlotRange(Capacity);
    }

    /// <summary> 빈 슬롯 없이 앞에서부터 채우기 </summary>
    public void TrimAll()
    {
        // 가장 빠른 배열 빈공간 채우기 알고리즘

        // i 커서와 j 커서
        // i 커서 : 가장 앞에 있는 빈칸을 찾는 커서
        // j 커서 : i 커서 위치에서부터 뒤로 이동하며 기존재 아이템을 찾는 커서

        // i커서가 빈칸을 찾으면 j 커서는 i+1 위치부터 탐색
        // j커서가 아이템을 찾으면 아이템을 옮기고, i 커서는 i+1 위치로 이동
        // j커서가 Capacity에 도달하면 루프 즉시 종료

        indexSetForUpdate.Clear();

        int i = -1;
        while (items[++i] != null) ;
        int j = i;

        while (true)
        {
            while (++j < Capacity && items[j] == null);

            if (j == Capacity)
                break;

            indexSetForUpdate.Add(i);
            indexSetForUpdate.Add(j);

            items[i] = items[j];
            items[j] = null;
            i++;
        }

        foreach (var index in indexSetForUpdate)
        {
            UpdateSlot(index);
        }
        inventoryUI.UpdateAllSlotFilters();
    }

    /// <summary> 빈 슬롯 없이 채우면서 아이템 종류별로 정렬하기 </summary>
    public void SortAll()
    {
        // 1. Trim
        int i = -1;
        while (items[++i] != null) ;
        int j = i;

        while (true)
        {
            while (++j < Capacity && items[j] == null) ;

            if (j == Capacity)
                break;

            items[i] = items[j];
            items[j] = null;
            i++;
        }

        // 2. Sort
        Array.Sort(items, 0, i, itemComparer);

        // 3. Update
        UpdateAllSlot();
        inventoryUI.UpdateAllSlotFilters(); // 필터 상태 업데이트
    }
}