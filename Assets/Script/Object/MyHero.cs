using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MyHero : Creature
{
    private MyHeroStateMachine _myHeroMachine;
    private NavMeshAgent _agent;
    protected HeroStatData _statData;
    public NavMeshAgent Agent { get { return _agent; } }
    public HeroStatData StatData { get { return _statData; } }
    public MyHeroStateMachine MyHeroStateMachine { get { return _myHeroMachine; } }
    protected override void Awake()
    {
        base.Awake();

        _myHeroMachine = new MyHeroStateMachine(this);
        _myHeroMachine.ChangeState(_myHeroMachine.IdleState);
        _statData = new HeroStatData();
        _agent = GetComponent<NavMeshAgent>();

        CameraController cameraController = Camera.main.GetComponent<CameraController>();
        cameraController.TargetTransform = transform;
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
