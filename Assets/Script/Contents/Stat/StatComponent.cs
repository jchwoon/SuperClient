using Google.Protobuf.Enum;
using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StatComponent
{
    private int _maxHp;
    private int _maxMp;
    private int _hp;
    private int _mp;
    private int _atkDamage;
    private int _defence;
    private float _moveSpeed;
    private float _atkSpeed;
    public int MaxHp { get; private set; }
    public int Hp { get; private set; }
    public int MaxMp { get; private set; }
    public int MP { get; private set; }
    public int AttackDamage { get; private set; }
    public int Defence {  get; private set; }
    public float MoveSpeed { get; private set; }
    public float AtkSpeed { get; private set; }

    public StatInfo StatInfo { get; private set; }

    public void InitStat(StatInfo statInfo)
    {
        MaxHp = statInfo.MaxHp;
        Hp = statInfo.Hp;
        MaxMp = statInfo.MaxMp;
        MP = statInfo.Mp;
        AttackDamage = statInfo.AtkDamage;
        Defence = statInfo.Defence;
        MoveSpeed = statInfo.MoveSpeed;
        AtkSpeed = statInfo.AtkSpeed;
        StatInfo = statInfo;
        StatInfo.MergeFrom(statInfo);
        UpdateStat();
    }

    public void UpdateStat()
    {
        Managers.EventBus.InvokeEvent(Enums.EventType.ChangeStat);
    }

    public void HandleModifyStat(StatInfo statInfo)
    {
        if (StatInfo == null)
            return;

        StatInfo.MergeFrom(statInfo);
        UpdateStat();
    }

    public void HandleModifyOneStat(EStatType statType, float value)
    {
        switch (statType)
        {
            case EStatType.Hp:
                StatInfo.Hp = (int)value;
                break;
            case EStatType.Mp:
                StatInfo.Mp = (int)value;
                break;
            case EStatType.MaxHp:
                StatInfo.MaxHp = (int)value;
                break;
            case EStatType.MaxMp:
                StatInfo.MaxMp = (int)value;
                break;
            case EStatType.Atk:
                StatInfo.AtkDamage = (int)value;
                break;
            case EStatType.Defence:
                StatInfo.Defence = (int)value;
                break;
            case EStatType.MoveSpeed:
                StatInfo.MoveSpeed = (int)value;
                break;
            case EStatType.AtkSpeed:
                StatInfo.AtkSpeed = (int)value;
                break;
            default:
                break;
        }
        UpdateStat();
    }
}
