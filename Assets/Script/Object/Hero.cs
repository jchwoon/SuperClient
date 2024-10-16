using Data;
using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Creature
{
    protected HeroStatData _statData;
    public HeroStatData StatData
    {
        get { return _statData; }
    }
    public HeroData HeroData { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        if (isMachineInit == false)
        {
            Machine = new HeroStateMachine(this);
            isMachineInit = true;
        }

        _statData = new HeroStatData();
    }
    protected override void Update()
    {
        base.Update();

    }

    public void Init(HeroInfo info, HeroData heroData)
    {
        HeroData = heroData;
        _statData.SetStat(info.CreatureInfo.StatInfo);
        SetInfo(info.CreatureInfo);
        SetPos(gameObject, info.CreatureInfo.ObjectInfo.PosInfo);
    }
}
