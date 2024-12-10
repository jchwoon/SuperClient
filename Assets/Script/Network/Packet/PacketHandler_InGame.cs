using Google.Protobuf.Protocol;
using Google.Protobuf;
using ServerCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf.Struct;
using Google.Protobuf.Enum;
using Data;

public partial class PacketHandler
{
    public static void SpawnToCHandler(PacketSession session, IMessage packet)
    {
        SpawnToC spawnPacket = (SpawnToC)packet;

        foreach (HeroInfo hero in spawnPacket.Heroes)
        {
            Managers.ObjectManager.Spawn(hero);
        }
        foreach (CreatureInfo creature in spawnPacket.Creatures)
        {
            Managers.ObjectManager.Spawn(creature);
        }
        foreach (ObjectInfo obj in spawnPacket.Objects)
        {
            Managers.ObjectManager.Spawn(obj);
        }
    }

    public static void MoveToCHandler(PacketSession session, IMessage packet)
    {
        MoveToC movePacket = (MoveToC)packet;

        GameObject go = Managers.ObjectManager.FindById(movePacket.ObjectId);

        if (go == null)
            return;
        BaseObject bo = go.GetComponent<BaseObject>();

        if (bo == null)
            return;

        bo.ReceivePosInfo(movePacket);
    }

    public static void DeSpawnToCHandler(PacketSession session, IMessage packet)
    {
        DeSpawnToC deSpawnPacket = (DeSpawnToC)packet;
        foreach (int id in deSpawnPacket.ObjectIds)
        {
            Managers.ObjectManager.DeSpawn(id);
        }

    }

    public static void ResUseSkillToCHandler(PacketSession session, IMessage packet)
    {
        ResUseSkillToC skillPacket = (ResUseSkillToC)packet;
        GameObject go = Managers.ObjectManager.FindById(skillPacket.ObjectId);

        if (go == null)
            return;

        Creature creature = go.GetComponent<Creature>();

        if (creature == null)
            return;

        creature.HandleUseSkill(creature, skillPacket);
    }

    public static void ModifyStatToCHandler(PacketSession session, IMessage packet)
    {
        ModifyStatToC statPacket = (ModifyStatToC)packet;
        GameObject go = Managers.ObjectManager.FindById(statPacket.ObjectId);

        if (go == null)
            return;

        Creature creature = go.GetComponent<Creature>();

        if (creature == null)
            return;

        creature.HandleModifyStat(statPacket.StatInfo);
    }

    public static void ModifyOneStatToCHandler(PacketSession session, IMessage packet)
    {
        ModifyOneStatToC statOnePacket = (ModifyOneStatToC)packet;
        GameObject go = Managers.ObjectManager.FindById(statOnePacket.ObjectId);

        if (go == null)
            return;

        Creature creature = go.GetComponent<Creature>();

        if (creature == null)
            return;

        creature.HandleModifyOneStat(statOnePacket.StatType, statOnePacket.ChangedValue, statOnePacket.GapValue, statOnePacket.FontType);
    }

    public static void DieToCHandler(PacketSession session, IMessage packet)
    {
        DieToC diePacket = (DieToC)packet;
        GameObject go = Managers.ObjectManager.FindById(diePacket.ObjectId);

        if (go == null)
            return;

        Creature creature = go.GetComponent<Creature>();

        if (creature == null)
            return;

        creature.HandleDie(diePacket.KillerId);
    }

    public static void TeleportToCHandler(PacketSession session, IMessage packet)
    {
        TeleportToC telpoPacket = (TeleportToC)packet;

        GameObject go = Managers.ObjectManager.FindById(telpoPacket.ObjectId);

        if (go == null)
            return;
        BaseObject bo = go.GetComponent<BaseObject>();

        if (bo == null)
            return;

        bo.HandleTeleport(telpoPacket);
    }

    public static void RewardToCHandler(PacketSession session, IMessage packet)
    {
        RewardToC rewardPacket = (RewardToC)packet;

        MyHero myHero = Managers.ObjectManager.MyHero;

        if (myHero == null)
            return;

        myHero.HandleReward(rewardPacket.Exp, rewardPacket.Gold);
    }

    public static void PickupDropItemToCHandler(PacketSession session, IMessage packet)
    {
        PickupDropItemToC pickupItemPacket = (PickupDropItemToC)packet;

        MyHero myHero = Managers.ObjectManager.MyHero;

        if (myHero == null)
            return;

        myHero.ResCheckCanPickup(pickupItemPacket.Result);
    }

    public static void AddItemToCHandler(PacketSession session, IMessage packet)
    {
        AddItemToC addItemPacket = (AddItemToC)packet;

        MyHero myHero = Managers.ObjectManager.MyHero;
        if (myHero == null)
            return;

        InventoryComponent inventory = myHero.Inventory;
        if (inventory == null)
            return;

        if (addItemPacket.AddType == EAddItemType.New)
        {
            Item item = Managers.ItemFactory.MakeItem(addItemPacket.ItemInfo);
            inventory.AddNewItem(item, invokeUpdate: true);
        }
        else
            inventory.AddItem(addItemPacket.ItemInfo);
    }

    public static void UseItemToCHandler(PacketSession session, IMessage packet)
    {
        UseItemToC useItemPacket = (UseItemToC)packet;

        MyHero myHero = Managers.ObjectManager.MyHero;
        if (myHero == null)
            return;

        InventoryComponent inventory = myHero.Inventory;
        if (inventory == null)
            return;

        Item item = inventory.FindItemByDbId(useItemPacket.ItemDbId);
        item.HandleUseItem(inventory);
    }

    public static void ChangeSlotTypeToCHandler(PacketSession session, IMessage packet)
    {
        ChangeSlotTypeToC changeSlotPacket = (ChangeSlotTypeToC)packet;

        MyHero myHero = Managers.ObjectManager.MyHero;
        if (myHero == null)
            return;

        InventoryComponent inventory = myHero.Inventory;
        if (inventory == null)
            return;

        inventory.HandleChangeSlotItem(changeSlotPacket.ItemDbId, changeSlotPacket.SlotType);
    }
    public static void ChangeRoomToSHandler(PacketSession session, IMessage packet)
    {
        ChangeRoomToS changeRoomToSPacket = (ChangeRoomToS)packet;

        MyHero myHero = Managers.ObjectManager.MyHero;
        if (myHero == null)
            return;
    }
    public static void ChangeRoomToCHandler(PacketSession session, IMessage packet)
    {
        ChangeRoomToS changeRoomToSPacket = (ChangeRoomToS)packet;

        MyHero myHero = Managers.ObjectManager.MyHero;
        if (myHero == null)
            return;
    }
    
}
