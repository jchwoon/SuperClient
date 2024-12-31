using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using Google.Protobuf.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Data;
using UnityEngine.InputSystem;

public class MyHero : Hero
{
    public MyHeroStateMachine MyHeroStateMachine { get; private set; }
    public SkillComponent SkillComponent { get; private set; }
    public CurrencyComponent CurrencyComponent { get; private set; }
    public GrowthComponent GrowthInfo { get; private set; }
    public InventoryComponent Inventory { get; private set; }
    public MyHeroInfo MyHeroInfo { get; private set; }

    protected override void Awake()
    {
        isMachineInit = true;
        base.Awake();

        CameraController cameraController = Camera.main.GetComponent<CameraController>();
        cameraController.Target = Utils.FindChild(gameObject, "HeadPoint");
        cameraController.transform.position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z + 5);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        //Temp
        Managers.EventBus.AddEvent(Enums.EventType.AtkBtnClick, OnAttackBtnClicked);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        
        //Temp
        Managers.EventBus.RemoveEvent(Enums.EventType.AtkBtnClick, OnAttackBtnClicked);
    }
    protected override void Update()
    {
        base.Update();
        //if (MyHeroStateMachine == null)
        //    return;
        //MyHeroStateMachine.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        BaseObject obj = other.GetComponent<BaseObject>();
        if (obj != null)
            obj.OnContactMyHero(this);
    }

    private void OnTriggerExit(Collider other)
    {
        BaseObject obj = other.GetComponent<BaseObject>();
        if (obj != null)
            obj.OnDetactMyHero(this);
    }

    public void Init(MyHeroInfo info, HeroData heroData)
    {
        MyHeroInfo = info;
        HeroInfo = info.HeroInfo;
        MyHeroStateMachine = new MyHeroStateMachine(this);
        SkillComponent = new SkillComponent();
        CurrencyComponent = new CurrencyComponent(this);
        GrowthInfo = new GrowthComponent(this);
        Inventory = new InventoryComponent(this);

        Machine = MyHeroStateMachine;
        HeroData = heroData;
        Name = info.HeroInfo.LobbyHeroInfo.Nickname;
        Stat.InitStat(info.HeroInfo.CreatureInfo.StatInfo);

        SetObjInfo(info.HeroInfo.CreatureInfo.ObjectInfo);
        SetPos(gameObject, info.HeroInfo.CreatureInfo.ObjectInfo.PosInfo);

        AddHUD();
    }

    private void OnAttackBtnClicked()
    {
        if (SkillComponent.isUsingSkill)
            return;
        BaseSkill normalSkill = SkillComponent.GetSkillById(SkillComponent.NormalSkillId);
        if (normalSkill.CheckCanUseSkill() == ESkillFailReason.None)
        {
            BaseObject target = MyHeroStateMachine.Target;
            SendUseSkill(normalSkill.TemplateId, target == null ? 0 : target.ObjectId);
        }
    }

    public void UseSkill(int skillTemplateId)
    {
        BaseSkill skill = SkillComponent.GetSkillById(skillTemplateId);
        if (SkillComponent.CheckCanUseSkill(skillTemplateId))
        {
            BaseObject target = MyHeroStateMachine.Target;
            SendUseSkill(skillTemplateId, target == null ? 0 : target.ObjectId);
        }
    }

    protected override void OnDie()
    {
        base.OnDie();
        Managers.UIManager.ShowFadeUI(HeroData.RespawnTime -1, false);
    }

    protected override void OnRevival()
    {
        base.OnRevival();
        Managers.UIManager.ShowFadeUI();
    }

    public override void HandleModifyStat(StatInfo statInfo)
    {
        base.HandleModifyStat(statInfo);
        Stat.UpdateStat();
    }

    public override void HandleModifyOneStat(EStatType statType, float changedValue, float gapValue, EFontType fontType)
    {
        Stat.SetStat(statType, changedValue);

        switch (statType)
        {
            case EStatType.Hp:
                Managers.EventBus.InvokeEvent(Enums.EventType.ChangeHUDInfo);
                FloatingTextController.RegisterOrSpawnText(gapValue, transform, fontType);
                break;
            case EStatType.Mp:
                Managers.EventBus.InvokeEvent(Enums.EventType.ChangeHUDInfo);
                break;
            default:
                break;
        }
    }

    public void HandleReward(int exp, int gold)
    {
        GrowthInfo.AddExp(exp);
        CurrencyComponent.AddGold(gold);

        FloatingTextController.RegisterOrSpawnText(exp, transform, EFontType.Exp, isReward:true);
        FloatingTextController.RegisterOrSpawnText(gold, transform, EFontType.Gold, isReward: true);
    }

    #region PickupItem
    public void ReqPickupItem(int objectId)
    {
        PickupDropItemToS pickupItemPacket = new PickupDropItemToS();
        pickupItemPacket.ObjectId = objectId;

        Managers.NetworkManager.Send(pickupItemPacket);
    }

    public void ResPickupItem(EPickupFailReason reason)
    {
        switch (reason)
        {
            case EPickupFailReason.Full:
                Managers.UIManager.ShowToasUI("인벤토리가 가득 찼습니다!");
                break;
                //만약 실패하지 않았으면 여기서 플레이어에게 가게하고
                //도착했으면 Despawn을 하면 괜찮을듯?
            default:
                break;
        }
    }
    #endregion
}
