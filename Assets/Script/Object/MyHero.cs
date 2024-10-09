using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using Google.Protobuf.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Data;

public class MyHero : Hero
{
    public MyHeroInfo Info { get; private set; }
    public MyHeroStateMachine MyHeroStateMachine { get; private set; }
    public SkillComponent SkillComponent { get; set; }

    protected override void Awake()
    {
        isMachineInit = true;
        base.Awake();
        MyHeroStateMachine = new MyHeroStateMachine(this);
        SkillComponent = new SkillComponent();
        Machine = MyHeroStateMachine;
        _statData = new HeroStatData();


        CameraController cameraController = Camera.main.GetComponent<CameraController>();
        cameraController.TargetTransform = transform;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Managers.EventBus.AddEvent(Enums.EventType.AtkBtnClick, OnAttackBtnClicked);
    }
    protected override void Update()
    {
        base.Update();
    }
    private void OnAttackBtnClicked()
    {
        MyHeroStateMachine.FindTargetAndAttack();
    }

    public void Init(MyHeroInfo info, HeroData heroData)
    {
        Info = info;
        HeroData = heroData;
        _statData.SetStat(info.HeroInfo.CreatureInfo.StatInfo);
        SetInfo(info.HeroInfo.CreatureInfo);
        SetPos(gameObject, info.HeroInfo.CreatureInfo.ObjectInfo.PosInfo);
        SkillComponent.RegisterSkill(heroData);
    }
}
