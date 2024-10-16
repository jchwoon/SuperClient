using Data;
using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Creature
{
    public HeroData HeroData { get; protected set; }
    public HeroInfo HeroInfo { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        if (isMachineInit == false)
        {
            Machine = new HeroStateMachine(this);
            isMachineInit = true;
        }
        base.Start();
    }
    protected override void Update()
    {
        base.Update();

    }

    public void Init(HeroInfo info, HeroData heroData)
    {
        HeroData = heroData;
        HeroInfo = info;
        StatInfo = info.CreatureInfo.StatInfo;
        Name = info.LobbyHeroInfo.Nickname;
        SetObjInfo(info.CreatureInfo);
        SetPos(gameObject, info.CreatureInfo.ObjectInfo.PosInfo);
    }
}
