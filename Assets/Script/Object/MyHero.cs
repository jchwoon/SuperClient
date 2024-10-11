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



        CameraController cameraController = Camera.main.GetComponent<CameraController>();
        cameraController.TargetTransform = transform;
        cameraController.transform.position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z + 5);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        Managers.EventBus.AddEvent(Enums.EventType.AtkBtnClick, OnAttackBtnClicked);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        Managers.EventBus.RemoveEvent(Enums.EventType.AtkBtnClick, OnAttackBtnClicked);
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
        MyHeroStateMachine = new MyHeroStateMachine(this);
        SkillComponent = new SkillComponent();
        Machine = MyHeroStateMachine;
        Info = info;
        HeroData = heroData;
        _statData.SetStat(info.HeroInfo.CreatureInfo.StatInfo);
        SetInfo(info.HeroInfo.CreatureInfo);
        SetPos(gameObject, info.HeroInfo.CreatureInfo.ObjectInfo.PosInfo);
        SkillComponent.RegisterSkill(heroData);
    }
}
