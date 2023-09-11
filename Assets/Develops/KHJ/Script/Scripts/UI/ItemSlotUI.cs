using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlotUI : MonoBehaviour, IPointerEnterHandler
{
    [Tooltip("슬롯 내에서 아이콘과 슬롯 사이의 여백")]
    [SerializeField] private float padding = 1f;

    [Tooltip("아이템 아이콘 이미지")]
    [SerializeField] private Image iconImage;

    [Tooltip("아이템 개수 텍스트")]
    [SerializeField] private Text amountText;
    
    /// <summary> 슬롯의 인덱스 </summary>
    public int Index { get; private set; }

    /// <summary> 슬롯이 아이템을 보유하고 있는지 여부 </summary>
    public bool HasItem => iconImage.sprite != null;

    /// <summary> 접근 가능한 슬롯인지 여부 </summary>
    public bool IsAccessible => isAccessibleSlot && isAccessibleItem;

    public RectTransform SlotRect => slotRect;
    public RectTransform IconRect => iconRect;

    private InventoryUI inventoryUI;

    private RectTransform slotRect;
    private RectTransform iconRect;

    private GameObject iconGo;
    private GameObject textGo;

    private Image slotImage;

    private bool isAccessibleSlot = true; // 슬롯 접근가능 여부
    private bool isAccessibleItem = true; // 아이템 접근가능 여부

    /// <summary> 비활성화된 슬롯의 색상 </summary>
    private static readonly Color InaccessibleSlotColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
    /// <summary> 비활성화된 아이콘 색상 </summary>
    private static readonly Color InaccessibleIconColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

    private void OnEnable()
    {
        InitComponents();
        InitValues();
    }

    private void InitComponents()
    {
        inventoryUI = GetComponentInParent<InventoryUI>();

        // Rects
        slotRect = GetComponent<RectTransform>();
        iconRect = iconImage.rectTransform;

        // Game Objects
        iconGo = iconRect.gameObject;
        textGo = amountText.gameObject;

        // Images
        slotImage = GetComponent<Image>();
    }

    private void InitValues()
    {
        // 1. Item Icon, Highlight Rect
        iconRect.pivot = new Vector2(0.5f, 0.5f); // 피벗은 중앙
        iconRect.anchorMin = Vector2.zero;        // 앵커는 Top Left
        iconRect.anchorMax = Vector2.one;

        // 패딩 조절
        iconRect.offsetMin = Vector2.one * (padding);
        iconRect.offsetMax = Vector2.one * (-padding);

        // 2. Image
        iconImage.raycastTarget = false;

        // 3. Deactivate Icon
        HideIcon();
    }

    private void ShowIcon() => iconGo.SetActive(true);
    private void HideIcon() => iconGo.SetActive(false);

    private void ShowText() => textGo.SetActive(true);
    private void HideText() => textGo.SetActive(false);

    public void SetSlotIndex(int index) => Index = index;

    /// <summary> 슬롯 자체의 활성화/비활성화 여부 설정 </summary>
    public void SetSlotAccessibleState(bool value)
    {
        // 중복 처리는 지양
        if (isAccessibleSlot == value) return;

        if (value)
        {
            slotImage.color = Color.black;
        }
        else
        {
            slotImage.color = InaccessibleSlotColor;
            HideIcon();
            HideText();
        }

        isAccessibleSlot = value;
    }

    /// <summary> 아이템 활성화/비활성화 여부 설정 </summary>
    public void SetItemAccessibleState(bool value)
    {
        // 중복 처리는 지양
        if(isAccessibleItem == value) return;

        if (value)
        {
            iconImage.color = Color.white;
            amountText.color = Color.white;
        }
        else
        {
            iconImage.color  = InaccessibleIconColor;
            amountText.color = InaccessibleIconColor;
        }

        isAccessibleItem = value;
    }

    /// <summary> 슬롯에 아이템 등록 </summary>
    public void SetItem(Sprite itemSprite)
    {
        //if (!this.IsAccessible) return;

        if (itemSprite != null)
        {
            iconImage.sprite = itemSprite;
            ShowIcon();
        }
        else
        {
            RemoveItem();
        }
    }

    /// <summary> 슬롯에서 아이템 제거 </summary>
    public void RemoveItem()
    {
        iconImage.sprite = null;
        HideIcon();
        HideText();
    }

    /// <summary> 아이템 이미지 투명도 설정 </summary>
    public void SetIconAlpha(float alpha)
    {
        iconImage.color = new Color(
            iconImage.color.r, iconImage.color.g, iconImage.color.b, alpha
        );
    }

    /// <summary> 아이템 개수 텍스트 설정(amount가 1 이하일 경우 텍스트 미표시) </summary>
    public void SetItemAmount(int amount)
    {
        //if (!this.IsAccessible) return;

        if (HasItem && amount > 1)
            ShowText();
        else
            HideText();

        amountText.text = amount.ToString();

    }

    /// <summary> 슬롯에서 아이템 사용 </summary>
    public void UseItem()
    {
        inventoryUI.TryUseItem(Index);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryUI.TrytooltipItem(Index);
    }
}
