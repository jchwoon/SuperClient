using Data;
using Google.Protobuf.Enum;
using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public ItemData ItemData { get; private set; }
    public ItemInfo Info { get; private set; } = new ItemInfo();
    public int Price { get; private set; }

    public long ItemDbId
    {
        get { return Info.ItemDbId; }
        set { Info.ItemDbId = value; }
    }

    public int Count
    {
        get { return Info.Count; }
        set
        {
            if (value <= 0)
                Managers.ObjectManager.MyHero.Inventory.RemoveItem(ItemDbId);
            Info.Count = value;
        }
    }

    public ESlotType SlotType
    {
        get { return Info.SlotType; }
        set { Info.SlotType = value; }
    }
    public Item(int itemId)
    {
        ItemData itemData;
        if (Managers.DataManager.ItemDict.TryGetValue(itemId, out itemData) == true)
            ItemData = itemData;
    }

    public virtual void HandleUseItem(InventoryComponent inventory)
    {
        
    }
}
