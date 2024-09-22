using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MyHero : Hero
{
    private MyHeroStateMachine _myHeroMachine;
    private NavMeshAgent _agent;
    public NavMeshAgent Agent { get { return _agent; } }
    public MyHeroStateMachine MyHeroStateMachine { get { return _myHeroMachine; } }
    protected override void Awake()
    {
        base.Awake();

        _myHeroMachine = new MyHeroStateMachine(this);
        Machine = _myHeroMachine;
        Machine.ChangeState(_myHeroMachine.IdleState);
        _statData = new HeroStatData();
        _agent = GetComponent<NavMeshAgent>();

        CameraController cameraController = Camera.main.GetComponent<CameraController>();
        cameraController.TargetTransform = transform;
    }
    protected override void Update()
    {
        base.Update();
    }
    public void SetInfo(MyHeroInfo myHeroInfo)
    {
        _statData.SetStat(myHeroInfo.HeroInfo.StatInfo);
        ObjectId = myHeroInfo.HeroInfo.ObjectInfo.ObjectId;
    }
}
