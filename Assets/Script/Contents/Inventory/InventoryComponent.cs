using Google.Protobuf.Struct;
using Google.Protobuf.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryComponent
{
    MyHero _owner;
    Dictionary<int, Item> _ownerAllItems = new Dictionary<int, Item>();
    //Equipped
    Dictionary<ESlotType, Equipment> _equippedItems = new Dictionary<ESlotType, Equipment>();

    //Inventory
    Dictionary<int, Item> _equipInvenItems = new Dictionary<int, Item>();
    Dictionary<int, Item> _consumeInvenItems = new Dictionary<int, Item>();
    Dictionary<int, Item> _etcInvenItems = new Dictionary<int, Item>();

    Dictionary<EConsumableType, long> _consumableCoolDict = new Dictionary<EConsumableType, long>();

    public InventoryComponent(MyHero owner)
    {
        _owner = owner;
    }

    public void InitInventory(List<ItemInfo> items)
    {
        foreach (ItemInfo info in items)
        {
            Item item = Managers.ItemFactory.MakeItem(info);
            if (item == null)
                continue;
            AddNewItem(item);
            if ((int)info.SlotType < (int)ESlotType.Equip)
            {
                AddEquippedItem(info.SlotType, (Equipment)item);
            }
        }
        InvokeUpdateInventory();
    }

    //새롭게 추가된 아이템
    public void AddNewItem(Item item, bool invokeUpdate = false)
    {
        
        _ownerAllItems.Add(item.ItemDbId, item);

        switch (item.ItemData.ItemType)
        {
            case EItemType.Consume:
                _consumeInvenItems.Add(item.ItemDbId, item);
                break;
            case EItemType.Equip:
                _equipInvenItems.Add(item.ItemDbId, item);
                break;
            case EItemType.Etc:
                _etcInvenItems.Add(item.ItemDbId, item);
                break;
        }

        if (invokeUpdate == true)
            InvokeUpdateInventory();
    }

    public void AddItem(ItemInfo info)
    {
        Item item = FindItemByDbId(info.ItemDbId);
        item.Count = info.Count;
        InvokeUpdateInventory();
    }

    public void InvokeUpdateInventory()
    {
        Managers.EventBus.InvokeEvent(Enums.EventType.UpdateInventory);
    }

    public List<Item> GetAllItemsInType(EItemType type)
    {
        List<Item> items = null;
        switch (type)
        {
            case EItemType.Consume:
                items = _consumeInvenItems.Values.ToList();
                break;
            case EItemType.Equip:
                items = _equipInvenItems.Values.ToList();
                break;
            case EItemType.Etc:
                items = _etcInvenItems.Values.ToList();
                break;
        }

        return items;
    }

    public List<Equipment> GetAllEquippedItem()
    {
        return _equippedItems.Values.ToList();
    }

    public List<Item> GetAllItems()
    {
        return _ownerAllItems.Values.ToList();
    }

    public Item FindItemByDbId(int dbId)
    {
        Item item;
        if (_ownerAllItems.TryGetValue(dbId, out item) == false)
            return null;

        return item;
    }

    public void RemoveItem(int itemDbId)
    {
        _ownerAllItems.Remove(itemDbId);
        _equipInvenItems.Remove(itemDbId);
        _consumeInvenItems.Remove(itemDbId);
        _etcInvenItems.Remove(itemDbId);
    }

    public void AddEquippedItem(ESlotType slotType, Equipment equipItem)
    {
        _equippedItems.Add(slotType, equipItem);
    }
    public void RemoveEquippedItem(ESlotType slotType)
    {
        _equippedItems.Remove(slotType);
    }

    public bool ItemIsEquipped(ESlotType slotType, Equipment checkItem)
    {
        if (checkItem == null) 
            return false;

        Equipment equipItem;
        if (_equippedItems.TryGetValue(slotType, out equipItem) == true)
        {
            if (equipItem.ItemDbId == checkItem.ItemDbId)
                return true;
        }

        return false;
    }

    #region CoolTime
    public void UpdateCoolTick(EConsumableType type, long updateTick)
    {
        if (_consumableCoolDict.ContainsKey(type) == false)
        {
            _consumableCoolDict.Add(type, updateTick);
            return;
        }
        else
        {
            _consumableCoolDict[type] = updateTick;
        }
    }

    public long GetCurrentCoolTick(EConsumableType type)
    {
        if (_consumableCoolDict.ContainsKey(type) == false)
            return 0;

        return _consumableCoolDict[type];
    }
    #endregion

    #region Handler
    public void HandleChangeSlotItem(int itemDbId, ESlotType changedSlot)
    {
        Item item = FindItemByDbId(itemDbId);
        Equipment equipItem = (Equipment)item;
        ESlotType prevSlot = equipItem.SlotType;
        equipItem.SlotType = changedSlot;
        if ((int)changedSlot < (int)ESlotType.Equip)
            AddEquippedItem(equipItem.SlotType, equipItem);
        else
            RemoveEquippedItem(prevSlot);

        InvokeUpdateInventory();
    }
    #endregion
}
