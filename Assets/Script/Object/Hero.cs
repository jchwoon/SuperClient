using Data;
using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Creature
{
    public HeroData HeroData { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        if (isMachineInit == false)
        {
            Machine = new HeroStateMachine(this);
            isMachineInit = true;
        }
    }
    protected override void Update()
    {
        base.Update();

    }

    public void Init(HeroInfo info, HeroData heroData)
    {
        HeroData = heroData;
        Stat.InitStat(info.CreatureInfo.StatInfo);
        SetObjInfo(info.CreatureInfo);
        SetPos(gameObject, info.CreatureInfo.ObjectInfo.PosInfo);
    }
}
