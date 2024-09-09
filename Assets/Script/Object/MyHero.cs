using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyHero : Creature
{
    private MyHeroStateMachine _myHeroMachine;
    protected HeroStatData _statData;
    public HeroStatData StatData
    {
        get { return _statData; }
    }
    public MyHeroStateMachine MyHeroStateMachine
    {
        get { return _myHeroMachine; }
    }
    protected override void Awake()
    {
        base.Awake();

        _myHeroMachine = new MyHeroStateMachine(this);
        _myHeroMachine.ChangeState(_myHeroMachine.IdleState);
        _statData = new HeroStatData();
    }
    protected override void Update()
    {
        base.Update();
        _myHeroMachine.Update();
    }
    public void SetInfo(MyHeroInfo myHeroInfo)
    {
        _statData.SetStat(myHeroInfo.HeroInfo.StatInfo);
    }
}
