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
    public MyHeroStateMachine MyHeroStateMachine { get; private set; }
    public SkillComponent SkillComponent { get; private set; }
    public CurrencyComponent CurrencyComponent { get; private set; }
    public GrowthComponent GrowthInfo { get; protected set; }
    public StatComponent Stat { get; protected set; }

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
        if (MyHeroStateMachine.AttackMode == true)
            MyHeroStateMachine.OffAttackMode();
        else
            MyHeroStateMachine.FindTargetAndAttack();
    }

    public void Init(MyHeroInfo info, HeroData heroData)
    {
        MyHeroStateMachine = new MyHeroStateMachine(this);
        SkillComponent = new SkillComponent();
        CurrencyComponent = new CurrencyComponent();
        Stat = new StatComponent(this);
        GrowthInfo = new GrowthComponent();

        Machine = MyHeroStateMachine;
        HeroData = heroData;
        Name = info.HeroInfo.LobbyHeroInfo.Nickname;

        GrowthInfo.InitGrowth(info.HeroInfo.LobbyHeroInfo.Level, info.Exp);
        CurrencyComponent.InitCurrency(info.Gold);
        StatInfo = info.HeroInfo.CreatureInfo.StatInfo;
        Stat.UpdateStat();
        SkillComponent.InitSkill(heroData);
        SetObjInfo(info.HeroInfo.CreatureInfo);
        SetPos(gameObject, info.HeroInfo.CreatureInfo.ObjectInfo.PosInfo);

        AddHUD();
    }
}
