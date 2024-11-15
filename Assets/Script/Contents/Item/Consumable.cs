using Data;
using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Consumable : Item
{
    public ConsumableData ConsumableData { get; private set; }
    public Consumable(int itemId) : base(itemId)
    {
        ConsumableData = (ConsumableData)ItemData;
    }

    public Enums.UseItemFailReason CheckAndUseItem()
    {
        Enums.UseItemFailReason failReason = CheckCanUseItem();

        if (failReason == Enums.UseItemFailReason.None)
            UseItem();

        return failReason;
    }

    private void UseItem()
    {
        UseItemToS useItemPacket = new UseItemToS();
        useItemPacket.ItemdDbId = ItemDbId;
        Managers.NetworkManager.Send(useItemPacket);
    }

    private Enums.UseItemFailReason CheckCanUseItem()
    {
        if (GetRemainSeconds() > 0)
            return Enums.UseItemFailReason.Cool;
        else if (ConsumableData.ConsumableType == EConsumableType.Hp && CheckIsFullHp() == true)
            return Enums.UseItemFailReason.FullHp;
        else if (ConsumableData.ConsumableType == EConsumableType.Mp && CheckIsFullMp() == true)
            return Enums.UseItemFailReason.FullMp;

        return Enums.UseItemFailReason.None;
    }

    public Tuple<float, float> GetRemainSecAndRatio()
    {
        float sec = GetRemainSeconds();
        float ratio = sec / ConsumableData.CoolTime;

        Tuple<float, float> t = new Tuple<float, float>(sec, ratio);
        return t;
    }

    private float GetRemainSeconds()
    {
        MyHero myHero = Managers.ObjectManager.MyHero;
        if (myHero == null)
            return float.MaxValue;

        InventoryComponent inventory = myHero.Inventory;
        if (inventory == null)
            return float.MaxValue;

        long coolTick = inventory.GetCurrentCoolTick(ConsumableData.ConsumableType);
        float sec = Mathf.Max(0, coolTick - Time.time * 1000) / 1000;

        return sec;
    }

    private bool CheckIsFullHp()
    {
        MyHero hero = Managers.ObjectManager.MyHero;
        if (hero == null)
            return true;

        int hp = (int)hero.Stat.GetStat(EStatType.Hp);
        int maxHp = (int)hero.Stat.GetStat(EStatType.MaxHp);

        if (hp >= maxHp)
            return true;

        return false;
    }

    private bool CheckIsFullMp()
    {
        MyHero hero = Managers.ObjectManager.MyHero;
        if (hero == null)
            return true;

        int mp = (int)hero.Stat.GetStat(EStatType.Mp);
        int maxMp = (int)hero.Stat.GetStat(EStatType.MaxMp);

        if (mp >= maxMp)
            return true;

        return false;
    }

    #region Handler
    public override void HandleUseItem(InventoryComponent inventory)
    {
        inventory.UpdateCoolTick(ConsumableData.ConsumableType, Utils.TickCount + (int)(ConsumableData.CoolTime * 1000));
    }
    #endregion
}
