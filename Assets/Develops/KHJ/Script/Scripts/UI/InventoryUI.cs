using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*
    [기능 - 에디터 전용]
    - 게임 시작 시 동적으로 생성될 슬롯 미리보기(개수, 크기 미리보기 가능)

    [기능 - 유저 인터페이스]
    - 슬롯에 마우스 올리기
      - 사용 가능 슬롯 : 하이라이트 이미지 표시
      - 아이템 존재 슬롯 : 아이템 정보 툴팁 표시

    - 드래그 앤 드롭
      - 아이템 존재 슬롯 -> 아이템 존재 슬롯 : 두 아이템 위치 교환
      - 아이템 존재 슬롯 -> 아이템 미존재 슬롯 : 아이템 위치 변경
        - Shift 또는 Ctrl 누른 상태일 경우 : 셀 수 있는 아이템 수량 나누기
      - 아이템 존재 슬롯 -> UI 바깥 : 아이템 버리기

    - 슬롯 우클릭
      - 사용 가능한 아이템일 경우 : 아이템 사용

    - 기능 버튼(좌측 상단)
      - Trim : 앞에서부터 빈 칸 없이 아이템 채우기
      - Sort : 정해진 가중치대로 아이템 정렬

    - 필터 버튼(우측 상단)
      - [A] : 모든 아이템 필터링
      - [E] : 장비 아이템 필터링
      - [P] : 소비 아이템 필터링

      * 필터링에서 제외된 아이템 슬롯들은 조작 불가

    [기능 - 기타]
    - InvertMouse(bool) : 마우스 좌클릭/우클릭 반전 여부 설정
*/

public class InventoryUI : MonoBehaviour
{ 
    [Header("Options")]
    [Space]
    [SerializeField] private bool showTooltip = true;
  
    [Header("Connected Objects")]
    [SerializeField] private RectTransform contentAreaRT; // 슬롯들이 위치할 영역
  
    [Header("Buttons")]
    [SerializeField] private Button trimButton;
    [SerializeField] private Button sortButton;
  
    [Header("Filter Toggles")]
    [SerializeField] private Toggle toggleFilterAll;
    [SerializeField] private Toggle toggleFilterEquipments;
    [SerializeField] private Toggle toggleFilterPortions;
  
    /// <summary> 연결된 인벤토리 </summary>
    private Inventory inventory;
  
    private List<ItemSlotUI> slotUIList = new List<ItemSlotUI>();
  
    /// <summary> 인벤토리 UI 내 아이템 필터링 옵션 </summary>
    private enum FilterOption
    {
        All, Equipment, Portion
    }
    private FilterOption currentFilterOption = FilterOption.All;
  
    private void Awake()
    {
        InitSlots();
        InitButtonEvents();
        InitToggleEvents();
    }
  
    private void InitSlots()
    {
        for (int i = 0; i < contentAreaRT.childCount; i++)
        {
            Transform itemSlotTransform = contentAreaRT.GetChild(i);
            ItemSlotUI itemSlot = itemSlotTransform.gameObject.GetComponent<ItemSlotUI>();
            itemSlot.SetSlotIndex(i);
            slotUIList.Add(itemSlot);
        }
    }
  
    private void InitButtonEvents()
    {
        trimButton.onClick.AddListener(() => inventory.TrimAll());
        sortButton.onClick.AddListener(() => inventory.SortAll());
    }
  
    private void InitToggleEvents()
    {
        toggleFilterAll.onValueChanged.AddListener(       flag => UpdateFilter(flag, FilterOption.All));
        toggleFilterEquipments.onValueChanged.AddListener(flag => UpdateFilter(flag, FilterOption.Equipment));
        toggleFilterPortions.onValueChanged.AddListener(  flag => UpdateFilter(flag, FilterOption.Portion));
  
        // Local Method
        void UpdateFilter(bool flag, FilterOption option)
        {
            if (flag)
            {
                currentFilterOption = option;
                UpdateAllSlotFilters();
            }
        }
    }
  
    /// <summary> 아이템 사용 </summary>
    public void TryUseItem(int index)
    {
        inventory.Use(index);
    }

    public void TrytooltipItem(int index)
    {
        inventory.ViewtooltipItem(index);
    }
  
    /// <summary> 인벤토리 참조 등록 (인벤토리에서 직접 호출) </summary>
    public void SetInventoryReference(Inventory inventory)
    {
        this.inventory = inventory;
    }
  
    /// <summary> 슬롯에 아이템 아이콘 등록 </summary>
    public void SetItemIcon(int index, Sprite icon)
    {
        slotUIList[index].SetItem(icon);
    }
  
    /// <summary> 해당 슬롯의 아이템 개수 텍스트 지정 </summary>
    public void SetItemAmountText(int index, int amount)
    {
        // NOTE : amount가 1 이하일 경우 텍스트 미표시
        slotUIList[index].SetItemAmount(amount);
    }
  
    /// <summary> 해당 슬롯의 아이템 개수 텍스트 지정 </summary>
    public void HideItemAmountText(int index)
    {
        slotUIList[index].SetItemAmount(1);
    }
  
    /// <summary> 슬롯에서 아이템 아이콘 제거, 개수 텍스트 숨기기 </summary>
    public void RemoveItem(int index)
    {
        slotUIList[index].RemoveItem();
    }
  
    /// <summary> 접근 가능한 슬롯 범위 설정 </summary>
    public void SetAccessibleSlotRange(int accessibleSlotCount)
    {
        for (int i = 0; i < slotUIList.Count; i++)
        {
            slotUIList[i].SetSlotAccessibleState(i < accessibleSlotCount);
        }
    }
  
    /// <summary> 특정 슬롯의 필터 상태 업데이트 </summary>
    public void UpdateSlotFilterState(int index, ItemData itemData)
    {
        bool isFiltered = true;
  
        // null인 슬롯은 타입 검사 없이 필터 활성화
        if(itemData != null)
            switch (currentFilterOption)
            {
                case FilterOption.Portion:
                    isFiltered = (itemData is PortionItemData);
                    break;
            }
  
        slotUIList[index].SetItemAccessibleState(isFiltered);
    }
  
    /// <summary> 모든 슬롯 필터 상태 업데이트 </summary>
    public void UpdateAllSlotFilters()
    {
        int capacity = inventory.Capacity;
  
        for (int i = 0; i < capacity; i++)
        {
            ItemData data = inventory.GetItemData(i);
            UpdateSlotFilterState(i, data);
        }
    }
}
