using Google.Protobuf.Enum;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EtcItemSlot : ItemSlot
{
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
        Get<TMP_Text>((int)Texts.ItemInfoTxt).text = $"{_item.Count}";
    }
}
