using System.Collections;
using Google.Protobuf.Enum;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Google.Protobuf.Protocol;

public class ConsumeItemSlot : ItemSlot
{
    Image _coolTimeImage;
    TMP_Text _coolTimeTxt;
    protected override void Awake()
    {
        base.Awake();
        _coolTimeImage = Get<GameObject>((int)GameObjects.CoolTimeOverlay).GetComponent<Image>();
        _coolTimeTxt = Get<TMP_Text>((int)Texts.CoolTimeTxt);
    }

    private void Update()
    {
        Consumable consumable = (Consumable)_item;
        if (consumable == null)
        {
            ClearCoolInfo();
            return;
        }

        if (consumable.ConsumableData.ConsumableType == EConsumableType.None)
            return;

        Tuple<float, float> t = consumable.GetRemainSecAndRatio();

        if (t.Item1 > 0)
        {
            _coolTimeImage.fillAmount = t.Item2;
            _coolTimeTxt.text = Mathf.CeilToInt(t.Item1).ToString();
        }
        else
        {
            ClearCoolInfo();
        }
    }

    public override void RefreshSlot()
    {
        base.RefreshSlot();


    }

    public override void RefreshItem(Item item)
    {
        base.RefreshItem(item);
        _itemInfoTxt.text = $"{item.Count}";
    }

    protected override void ShowInteractBtnAndTooltip()
    {
        base.ShowInteractBtnAndTooltip();
        Get<TMP_Text>((int)Texts.InteractTxt).text = "���";
    }

    protected override void OnInteractOverlayClicked()
    {
        base.OnInteractOverlayClicked();
        Consumable consumable = (Consumable)_item;
        Enums.UseItemFailReason failReason = consumable.CheckAndUseItem();

        switch (failReason)
        {
            case Enums.UseItemFailReason.Cool:
                Managers.UIManager.ShowToasUI("���� ��� �ð� �߿��� ����� �� �����ϴ�.");
                break;
            case Enums.UseItemFailReason.FullHp:
                Managers.UIManager.ShowToasUI("HP�� �ִ��� ���¿����� ����� �� �����ϴ�.");
                break;
            case Enums.UseItemFailReason.FullMp:
                Managers.UIManager.ShowToasUI("MP�� �ִ��� ���¿����� ����� �� �����ϴ�.");
                break;
        }
    }

    private void ClearCoolInfo()
    {
        _coolTimeImage.fillAmount = 0;
        _coolTimeTxt.text = "";
    }
}
