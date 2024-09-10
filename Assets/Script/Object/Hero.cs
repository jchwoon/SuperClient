using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Creature
{
    private HeroStateMachine _heroMachine;
    protected HeroStatData _statData;

    protected override void Awake()
    {
        base.Awake();

        _heroMachine = new HeroStateMachine(this);
        _statData = new HeroStatData();
        _heroMachine.ChangeState(_heroMachine.IdleState);
    }
    protected override void Update()
    {
        base.Update();

        _heroMachine.Update();
    }

    public void SetInfo(HeroInfo heroInfo)
    {
        _statData.SetStat(heroInfo.StatInfo);
    }
}
