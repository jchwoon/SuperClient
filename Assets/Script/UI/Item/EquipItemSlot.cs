using Google.Protobuf.Enum;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EquipItemSlot : ItemSlot
{
    private Equipment _equipItem;
    protected override void Awake()
    {
        base.Awake();
    }

    public override void RefreshSlot()
    {
        base.RefreshSlot();
    }
    public override void RefreshItem(Item item)
    {
        base.RefreshItem(item);
        _equipItem = (Equipment)item;
        if ((int)_item.SlotType < (int)ESlotType.Equip)
            Get<TMP_Text>((int)Texts.ItemInfoTxt).text = $"E";
    }

    protected override void ShowInteractBtnAndTooltip()
    {
        base.ShowInteractBtnAndTooltip();
        TMP_Text interactTxt = Get<TMP_Text>((int)Texts.InteractTxt);

        if ((int)_item.SlotType < (int)ESlotType.Equip)
            interactTxt.text = "해제";
        else
            interactTxt.text = "장착";
    }

    protected override void OnInteractOverlayClicked()
    {
        base.OnInteractOverlayClicked();
        if (_equipItem == null)
            return;

        if ((int)_item.SlotType < (int)ESlotType.Equip)
        {
            _equipItem.SendUnEquipItem();
        }
        else
        {
            bool canEquip = _equipItem.CheckEquip();
            if (canEquip == true)
                _equipItem.SendEquipItem();
            else
                Managers.UIManager.ShowToasUI("해당 아이템을 착용하기 위해 필요한 조건을 충족하지 않았습니다.");
        }            
    }
}
