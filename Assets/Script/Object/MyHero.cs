using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MyHero : Hero
{
    public MyHeroStateMachine MyHeroStateMachine { get; set; }
    protected override void Awake()
    {
        isMachineInit = true;
        base.Awake();
        MyHeroStateMachine = new MyHeroStateMachine(this);
        Machine = MyHeroStateMachine;
        _statData = new HeroStatData();

        CameraController cameraController = Camera.main.GetComponent<CameraController>();
        cameraController.TargetTransform = transform;
    }
    protected override void Update()
    {
        base.Update();
    }
    public void SetInfo(MyHeroInfo myHeroInfo)
    {
        _statData.SetStat(myHeroInfo.HeroInfo.CreatureInfo.StatInfo);
        ObjectId = myHeroInfo.HeroInfo.CreatureInfo.ObjectInfo.ObjectId;
    }
}
