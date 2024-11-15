using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : BaseUI
{
    protected enum Images
    {
        Item
    }
    protected enum Texts
    {
        ItemInfoTxt,
        InteractTxt,
        CoolTimeTxt
    }
    protected enum GameObjects
    {
        InteractOverlay,
        CoolTimeOverlay
    }

    protected Item _item;
    protected bool _isInit;
    private Vector2 _preDragPos;
    protected GameObject _interactOverlay;
    protected Image _itemImage;
    protected TMP_Text _itemInfoTxt;
    protected ItemInfoTooltipUI _tooltip;
    protected override void Awake()
    {
        base.Awake();

        Bind<Image>(typeof(Images));
        Bind<TMP_Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        _interactOverlay = Get<GameObject>((int)GameObjects.InteractOverlay);
        _itemImage = Get<Image>((int)Images.Item);
        _itemInfoTxt = Get<TMP_Text>((int)Texts.ItemInfoTxt);

        BindEvent(gameObject, OnSlotClicked);
        BindEvent(gameObject, OnSlotDrag, Enums.TouchEvent.Drag);
        BindEvent(gameObject, OnSlotPointerDown, Enums.TouchEvent.PointerDown);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public void OnSlotPointerDown(PointerEventData eventData)
    {
        _preDragPos = eventData.position;
    }

    public void OnSlotDrag(PointerEventData eventData)
    {
        Vector3 dragDirection = eventData.position - _preDragPos;

        transform.parent.localPosition += new Vector3(0, dragDirection.y, 0) * 100 * Time.deltaTime;

        _preDragPos = eventData.position;
    }

    public virtual void RefreshSlot()
    {
        Clear();
        gameObject.SetActive(true);
    }

    public virtual void RefreshItem(Item item)
    {
        _item = item;
        Sprite sprite = Managers.ResourceManager.GetResource<Sprite>(_item.ItemData.ImageName);
        _itemImage.sprite = sprite;
        _itemImage.color = Color.white;
    }

    private void OnSlotClicked(PointerEventData eventData)
    {
        if (_item == null)
            return;

        if (_interactOverlay.activeSelf == false)
        {
            InventoryUI inven = Managers.UIManager.GetPopupUI<InventoryUI>();

            if (inven == null)
                return;

            inven.OnSlotClicked(this);
            ShowInteractBtnAndTooltip();
        }
        else
            OnInteractOverlayClicked();
    }

    protected virtual void OnInteractOverlayClicked()
    {
        _tooltip.Close();
        DeActiveInteractOverlay();
    }

    protected virtual void ShowInteractBtnAndTooltip()
    {
        _interactOverlay.SetActive(true);
        //Tooltip È°¼ºÈ­
        _tooltip = Managers.UIManager.ShowPopup<ItemInfoTooltipUI>();

        RectTransform rect = transform.GetComponent<RectTransform>();
        float slotHalfSize = rect.sizeDelta.x / 2;

        _tooltip.SetInfo(_item.ItemData, rect.position, slotHalfSize, DeActiveInteractOverlay);
    }

    public void DeActiveInteractOverlay()
    {
        _interactOverlay?.SetActive(false);
    }

    public virtual void Clear()
    {
        if (_item != null)
        {
            _item = null;
            _itemImage.sprite = null;
            _itemImage.color = Color.clear;
            _itemInfoTxt.text = "";
        }

    }
}
