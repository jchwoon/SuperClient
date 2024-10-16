using Data;
using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Google.Protobuf.Enum;

public class Creature : BaseObject
{
    CreatureHUD _creatureHUD;
    private bool _isTargetted;
    public bool IsTargetted
    {
        get { return _isTargetted; }
        set
        {
            if (value == true)
            {
                AddHUD();
                GetComponent<TargetOutlineController>().AddOutline(this);
            }    
            else
            {
                RemoveHUD();
                GetComponent<TargetOutlineController>().BackToOriginMats(this);
            }
            _isTargetted = value;
        }

    }

    public Animator Animator { get; private set; }
    public AnimationData AnimData { get; private set; }
    public StatInfo StatInfo { get; protected set; }
    public string Name { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        Animator = transform.GetComponent<Animator>();
        AnimData = new AnimationData();
    }
    protected override void Start()
    {
        if (isMachineInit == false)
        {
            Machine = new CreatureMachine(this);
            isMachineInit = true;
        }
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (_isTargetted == true && Managers.ObjectManager.MyHero)
        {
            Managers.ObjectManager.MyHero.MyHeroStateMachine.Target = null;

        }
    }

    protected void AddHUD()
    {
        if (_creatureHUD == null)
            _creatureHUD = Managers.UIManager.AddCreatureHUD(this);

        _creatureHUD.SetInfo(this);
    }
    protected void RemoveHUD()
    {
        if (_creatureHUD == null)
            return;

        _creatureHUD.RemoveHUD();
    }

    #region Network Send
    public void SendReqUseSkill(int skillId, int targetId = 0)
    {
        ReqUseSkillToS skillPacket = new ReqUseSkillToS();
        skillPacket.SkillId = skillId;
        skillPacket.TargetId = targetId;
        Managers.NetworkManager.Send(skillPacket);
    }
    #endregion

    #region Network Receive
    public void HandleResUseSkill(Creature owner, ResUseSkillToC skillPacket)
    {
        SkillData skillData;
        if (Managers.DataManager.SkillDict.TryGetValue(skillPacket.SkillId, out skillData) == false)
            return;

        Creature target = Managers.ObjectManager.FindById(skillPacket.TargetId).GetComponent<Creature>();
        if (target != null)
            owner.Machine.UseSkill(skillData, target);
    }

    public void HandleModifyOneStat(EStatType statType, float value)
    {
        switch (statType)
        {
            case EStatType.Hp:
                StatInfo.Hp = (int)value;
                break;
            case EStatType.Mp:
                StatInfo.Mp = (int)value;
                break;
            default:
                break;
        }

        Managers.EventBus.InvokeEvent(Enums.EventType.ChangeHUDInfo);
    }
    #endregion
}
