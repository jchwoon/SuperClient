using Data;
using Google.Protobuf.Enum;
using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory
{
    public Item MakeItem(ItemInfo info)
    {
        int templateId = info.TemplateId;

        ItemData itemData;
        if (Managers.DataManager.ItemDict.TryGetValue(templateId, out itemData) == false)
            return null;

        Item item = null;

        switch (itemData.ItemType)
        {
            case EItemType.Consume:
                item = new Consumable(templateId);
                break;
            case EItemType.Equip:
                item = new Equipment(templateId);
                break;
            case EItemType.Etc:
                item = new Etc(templateId);
                break;
        }

        if (item != null)
        {
            item.ItemDbId = info.ItemDbId;
            item.Count = info.Count;
            item.SlotType = info.SlotType;
        }

        return item;
    }
}
