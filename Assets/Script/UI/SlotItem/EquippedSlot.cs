using Google.Protobuf.Enum;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquippedSlot : ItemSlot
{
    [SerializeField]
    public ESlotType slotType;

    Equipment _equipItem;
    TMP_Text _interactTxt;

    protected override void Awake()
    {
        base.Awake();
        _interactTxt = Get<TMP_Text>((int)Texts.InteractTxt);
    }

    public void SetInfo(Equipment equipItem)
    {
        _equipItem = equipItem;
        _item = equipItem;
        Sprite sprite = Managers.ResourceManager.GetResource<Sprite>(equipItem.ItemData.ImageName);
        _itemImage.sprite = sprite;
        _itemImage.color = Color.white;
    }

    public override void Clear()
    {
        if (_equipItem != null)
        {
            _item = null;
            _equipItem = null;
            _itemImage.sprite = null;
            _itemImage.color = Color.clear;
        }
    }

    protected override void OnInteractOverlayClicked()
    {
        base.OnInteractOverlayClicked();
        if (_equipItem == null)
            return;

        _equipItem.SendUnEquipItem();
    }

    protected override void ShowInteractBtnAndTooltip()
    {
        base.ShowInteractBtnAndTooltip();
        _interactTxt.text = "«ÿ¡¶";
    }
}
