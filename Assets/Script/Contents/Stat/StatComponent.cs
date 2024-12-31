using Data;
using Google.Protobuf.Enum;
using Google.Protobuf.Struct;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StatComponent
{
    public StatInfo StatInfo { get; set; } = new StatInfo();
    public Creature Owner { get; private set; }

    public StatComponent(Creature owner)
    {
        Owner = owner;
    }

    public static readonly Dictionary<EStatType, Action<StatInfo, float>> SetStatDict = new Dictionary<EStatType, Action<StatInfo, float>>()
    {
        {EStatType.Hp, (info, value) => { info.Hp = (int)value; } },
        {EStatType.MaxHp, (info, value) => { info.MaxHp =(int) value; } },
        {EStatType.Mp, (info, value) => { info.Mp =(int) value; } },
        {EStatType.MaxMp, (info, value) => { info.MaxMp =(int) value; } },
        {EStatType.Atk, (info, value) => { info.AtkDamage =(int) value; } },
        {EStatType.Defence, (info, value) => { info.Defence =(int) value; } },
        {EStatType.MoveSpeed, (info, value) => { info.MoveSpeed = value; } },
    };
    public static readonly Dictionary<EStatType, Func<StatInfo, float>> GetStatDict = new Dictionary<EStatType, Func<StatInfo, float>>()
    {
        {EStatType.Hp, (info) => { return info.Hp; } },
        {EStatType.MaxHp, (info) => { return info.MaxHp; } },
        {EStatType.Mp, (info) => { return info.Mp; } },
        {EStatType.MaxMp, (info) => { return info.MaxMp; } },
        {EStatType.Atk, (info) => { return info.AtkDamage; } },
        {EStatType.Defence, (info) => { return info.Defence; } },
        {EStatType.MoveSpeed, (info) => { return info.MoveSpeed; } },
    };

    public float GetStat(EStatType statType)
    {
        return GetStatDict[statType].Invoke(StatInfo);
    }
    public void InitStat(StatInfo statInfo)
    {
        StatInfo = statInfo;
    }

    public void SetStat(EStatType statType, float changedValue)
    {
        SetStatDict[statType].Invoke(StatInfo, changedValue);
        UpdateStat();
    }

    public void UpdateStat()
    {
        if (Owner.ObjectId == Managers.ObjectManager.MyHero.ObjectId)
        {
            Managers.EventBus.InvokeEvent(Enums.EventType.ChangeStat);
        }
    }
}
