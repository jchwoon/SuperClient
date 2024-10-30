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
        {EStatType.AtkSpeed, (info, value) => { info.AtkSpeed = value; } },
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
        {EStatType.AtkSpeed, (info) => { return info.AtkSpeed; } },
        {EStatType.MoveSpeed, (info) => { return info.MoveSpeed; } },
    };

    public float GetStat(EStatType statType)
    {
        return GetStatDict[statType].Invoke(StatInfo);
    }
    public virtual void InitStat(StatInfo statInfo)
    {
        StatInfo = statInfo;
        UpdateStat();
    }

    public void SetStat(EStatType statType, float changedValue)
    {
        SetStatDict[statType].Invoke(StatInfo, changedValue);
        UpdateStat();
    }

    public virtual void UpdateStat()
    {
        Owner.Animator.SetFloat(Owner.AnimData.AttackSpeedHash, StatInfo.AtkSpeed);
    }
}
