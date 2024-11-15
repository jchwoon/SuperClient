using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInfoTooltipUI : PopupUI
{
    enum Texts
    {
        NameTxt,

    }
    enum GameObjects
    {
        TextBox,
        CloseBtn,
        Tooltip
    }

    GameObject _infoBox;
    ItemData _itemData;
    GameObject _tooltip;
    Action _clearAction;
    protected override void Awake()
    {
        base.Awake();
        Bind<GameObject>(typeof(GameObjects));
        Bind<TMP_Text>(typeof(Texts));

        _infoBox = Get<GameObject>((int)GameObjects.TextBox);
        _tooltip = Get<GameObject>((int)GameObjects.Tooltip);

        BindEvent(Get<GameObject>((int)GameObjects.CloseBtn), OnCloseBtnClicked);
    }

    public void SetInfo(ItemData itemData, Vector2 slotPos, float slotHalfSize, Action clearAction)
    {
        _itemData = itemData;
        _clearAction = clearAction;
        SetPos(slotPos, slotHalfSize);
        Refresh();
    }

    private void Refresh()
    {
        for (int i = 0; i < _infoBox.transform.childCount; i++)
        {
            _infoBox.transform.GetChild(i).gameObject.SetActive(false);
        }
        int count = 0;
        //이름
        SetName();
        if (_itemData.ItemType == Google.Protobuf.Enum.EItemType.Equip)
        {
            //요구 클래스
            SetReqClass(ref count);
            //요구레벨
            SetReqLevel(ref count);
        }
        //효과
        SetEffectDesc(ref count);
        //쿨타임
        SetCoolDesc(ref  count);
        //설명
        SetDesc(ref count);
    }

    private void SetPos(Vector2 slotPos, float slotHalfSize)
    {
        RectTransform canvasRect = GetComponent<RectTransform>();
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            slotPos,
            null,
            out localPoint
        );

        Vector2 pivot = new Vector2
        (
            localPoint.x > 0 ? 1 : 0,
            localPoint.y > 0 ? 1 : 0
        );

        Vector2 offset = new Vector2
        (
            (localPoint.x > 0 ? -1 : 1) * slotHalfSize,
            (localPoint.y > 0 ? 1 : -1) * slotHalfSize
        );

        RectTransform tooltipRect = _tooltip.GetComponent<RectTransform>();
        tooltipRect.pivot = pivot;
        tooltipRect.localPosition = localPoint + offset;
    }

    private void SetName()
    {
        Get<TMP_Text>((int)Texts.NameTxt).text = _itemData.Name;
    }

    private void SetReqClass(ref int count)
    {
        EquipmentData equipData = (EquipmentData)_itemData;
        string descTxt = $"클래스 {equipData.ClassType.ToString()}";
        _infoBox.transform.GetChild(count).GetComponent<DescSlot>().SetInfo(descTxt);
        count++;
    }

    private void SetReqLevel(ref int count)
    {
        EquipmentData equipData = (EquipmentData)_itemData;
        string descTxt = $"요구 레벨 {equipData.RequiredLevel.ToString()}";
        _infoBox.transform.GetChild(count).GetComponent<DescSlot>().SetInfo(descTxt);
        count++;
    }

    private void SetDesc(ref int count)
    {
        if (Managers.DataManager.DescriptionDict.TryGetValue(_itemData.DescId, out DescriptionData desc) == true)
        {
            _infoBox.transform.GetChild(count).GetComponent<DescSlot>().SetInfo(desc.Text);
            count++;
        }
    }

    private void SetEffectDesc(ref int count)
    {
        if (Managers.DataManager.EffectDict.TryGetValue(_itemData.EffectId, out EffectData effect) == true)
        {
            foreach (AddStatInfo info in effect.AddStatValues)
            {

                string valueTxt = info.Value > 0 ? $"+{info.Value}" : $"{info.Value}";
                if (info.Multiplier == true)
                    valueTxt = $"{info.Value}%";
                string builtTxt = $"{Utils.GetStatTypeText(info.StatType)} {valueTxt}";
                _infoBox.transform.GetChild(count).GetComponent<DescSlot>().SetInfo(builtTxt);
                count++;
            }
        }
    }

    private void SetCoolDesc(ref int count)
    {
        ConsumableData consumeData = _itemData as ConsumableData;
        if (consumeData != null)
        {
            _infoBox.transform.GetChild(count).GetComponent<DescSlot>().SetInfo($"쿨타임 {consumeData.CoolTime}");
            count++;
        }
    }
    private void OnCloseBtnClicked(PointerEventData eventData)
    {
        Close();
    }

    public void Close()
    {
        ClosePopup<ItemInfoTooltipUI>();
        _clearAction?.Invoke();
    }
}
