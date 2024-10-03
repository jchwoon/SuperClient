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

    public void SetInfo(HeroInfo heroInfo)
    {
        _statData.SetStat(heroInfo.CreatureInfo.StatInfo);
        ObjectId = heroInfo.CreatureInfo.ObjectInfo.ObjectId;
    }
}
