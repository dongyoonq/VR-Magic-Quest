using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlotUI : MonoBehaviour
{
    /***********************************************************************
    *                               Option Fields
    ***********************************************************************/
    #region .
    [Tooltip("슬롯 내에서 아이콘과 슬롯 사이의 여백")]
    [SerializeField] private float _padding = 1f;

    [Tooltip("아이템 아이콘 이미지")]
    [SerializeField] private Image _iconImage;

    [Tooltip("아이템 개수 텍스트")]
    [SerializeField] private Text _amountText;
    #endregion
    /***********************************************************************
    *                               Properties
    ***********************************************************************/
    #region .
    /// <summary> 슬롯의 인덱스 </summary>
    public int Index { get; private set; }

    /// <summary> 슬롯이 아이템을 보유하고 있는지 여부 </summary>
    public bool HasItem => _iconImage.sprite != null;

    /// <summary> 접근 가능한 슬롯인지 여부 </summary>
    public bool IsAccessible => _isAccessibleSlot && _isAccessibleItem;

    public RectTransform SlotRect => _slotRect;
    public RectTransform IconRect => _iconRect;

    #endregion
    /***********************************************************************
    *                               Fields
    ***********************************************************************/
    #region .
    private InventoryUI _inventoryUI;

    private RectTransform _slotRect;
    private RectTransform _iconRect;

    private GameObject _iconGo;
    private GameObject _textGo;

    private Image _slotImage;

    private bool _isAccessibleSlot = true; // 슬롯 접근가능 여부
    private bool _isAccessibleItem = true; // 아이템 접근가능 여부

    /// <summary> 비활성화된 슬롯의 색상 </summary>
    private static readonly Color InaccessibleSlotColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
    /// <summary> 비활성화된 아이콘 색상 </summary>
    private static readonly Color InaccessibleIconColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

    #endregion
    /***********************************************************************
    *                               Unity Events
    ***********************************************************************/
    #region .
    private void Awake()
    {
        InitComponents();
        InitValues();
    }

    #endregion
    /***********************************************************************
    *                               Private Methods
    ***********************************************************************/
    #region .
    private void InitComponents()
    {
        _inventoryUI = GetComponentInParent<InventoryUI>();

        // Rects
        _slotRect = GetComponent<RectTransform>();
        _iconRect = _iconImage.rectTransform;

        // Game Objects
        _iconGo = _iconRect.gameObject;
        _textGo = _amountText.gameObject;

        // Images
        _slotImage = GetComponent<Image>();
    }

    private void InitValues()
    {
        // 1. Item Icon, Highlight Rect
        _iconRect.pivot = new Vector2(0.5f, 0.5f); // 피벗은 중앙
        _iconRect.anchorMin = Vector2.zero;        // 앵커는 Top Left
        _iconRect.anchorMax = Vector2.one;

        // 패딩 조절
        _iconRect.offsetMin = Vector2.one * (_padding);
        _iconRect.offsetMax = Vector2.one * (-_padding);

        // 2. Image
        _iconImage.raycastTarget = false;

        // 3. Deactivate Icon
        HideIcon();
    }

    private void ShowIcon() => _iconGo.SetActive(true);
    private void HideIcon() => _iconGo.SetActive(false);

    private void ShowText() => _textGo.SetActive(true);
    private void HideText() => _textGo.SetActive(false);

    #endregion
    /***********************************************************************
    *                               Public Methods
    ***********************************************************************/
    #region .

    public void SetSlotIndex(int index) => Index = index;

    /// <summary> 슬롯 자체의 활성화/비활성화 여부 설정 </summary>
    public void SetSlotAccessibleState(bool value)
    {
        // 중복 처리는 지양
        if (_isAccessibleSlot == value) return;

        if (value)
        {
            _slotImage.color = Color.black;
        }
        else
        {
            _slotImage.color = InaccessibleSlotColor;
            HideIcon();
            HideText();
        }

        _isAccessibleSlot = value;
    }

    /// <summary> 아이템 활성화/비활성화 여부 설정 </summary>
    public void SetItemAccessibleState(bool value)
    {
        // 중복 처리는 지양
        if(_isAccessibleItem == value) return;

        if (value)
        {
            _iconImage.color = Color.white;
            _amountText.color = Color.white;
        }
        else
        {
            _iconImage.color  = InaccessibleIconColor;
            _amountText.color = InaccessibleIconColor;
        }

        _isAccessibleItem = value;
    }

    /// <summary> 슬롯에 아이템 등록 </summary>
    public void SetItem(Sprite itemSprite)
    {
        //if (!this.IsAccessible) return;

        if (itemSprite != null)
        {
            _iconImage.sprite = itemSprite;
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
        _iconImage.sprite = null;
        HideIcon();
        HideText();
    }

    /// <summary> 아이템 이미지 투명도 설정 </summary>
    public void SetIconAlpha(float alpha)
    {
        _iconImage.color = new Color(
            _iconImage.color.r, _iconImage.color.g, _iconImage.color.b, alpha
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

        _amountText.text = amount.ToString();

    }

    /// <summary> 슬롯에서 아이템 사용 </summary>
    public void UseItem()
    {
        _inventoryUI.TryUseItem(Index);
    }
    #endregion
}
