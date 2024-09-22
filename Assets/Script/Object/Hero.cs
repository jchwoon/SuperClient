using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Creature
{
    private HeroStateMachine _heroMachine;
    protected HeroStatData _statData;
    public HeroStatData StatData
    {
        get { return _statData; }
    }
    public HeroStateMachine HeroMachine
    { 
        get { return _heroMachine; }
    }

    protected override void Awake()
    {
        base.Awake();

        _heroMachine = new HeroStateMachine(this);
        Machine = _heroMachine;
        Machine.ChangeState(_heroMachine.IdleState);
        _statData = new HeroStatData();
    }
    protected override void Update()
    {
        base.Update();

    }

    public void SetInfo(HeroInfo heroInfo)
    {
        _statData.SetStat(heroInfo.StatInfo);
        ObjectId = heroInfo.ObjectInfo.ObjectId;
    }
}
