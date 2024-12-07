using Data;
using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Google.Protobuf.Enum;
using System;
using static UnityEngine.UI.GridLayoutGroup;

public class Creature : BaseObject
{
    CreatureHUD _creatureHUD;

    public bool IsTargetted { get; private set; }
    public Animator Animator { get; private set; }
    public AnimationData AnimData { get; private set; }
    public StatComponent Stat { get; protected set; }
    public FloatingTextController FloatingTextController { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        Animator = transform.GetComponent<Animator>();
        AnimData = new AnimationData();
        FloatingTextController = GetComponent<FloatingTextController>();
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
        ClearTarget();
    }

    public void OnTargetted()
    {
        if (IsTargetted)
            return;
        GetComponent<TargetOutlineController>().AddOutline(this);
        IsTargetted = true;
        InvokeChangeHUD();
    }

    public void ClearTarget()
    {
        if (IsTargetted == true && Managers.ObjectManager.MyHero)
        {
            GetComponent<TargetOutlineController>().BackToOriginMats(this);
            IsTargetted = false;
            InvokeChangeHUD();
        }
    }

    protected virtual void OnDie()
    {
        Machine.OnDie();
        ClearTarget();
        RemoveHUD();
    }

    protected override void OnRevival()
    {
        base.OnRevival();

    }

    protected void AddHUD()
    {
        if (_creatureHUD == null)
            _creatureHUD = Managers.UIManager.AddCreatureHUD(this);

        _creatureHUD.AddHUD(this);
    }
    protected void RemoveHUD()
    {
        if (_creatureHUD == null)
            return;

        _creatureHUD.RemoveHUD();
    }

    public void InvokeChangeHUD()
    {
        Managers.EventBus.InvokeEvent(Enums.EventType.ChangeHUDInfo);
    }

    #region Network Send
    public void SendUseSkill(int skillId, int targetId)
    {
        ReqUseSkillToS skillPacket = new ReqUseSkillToS();
        SkillInfo skillInfo = new SkillInfo()
        {
            SkillId = skillId,
            TargetId = targetId,
            RotY = transform.rotation.eulerAngles.y
        };
        skillPacket.SkillInfo = skillInfo;
        Managers.NetworkManager.Send(skillPacket);
    }
    #endregion

    #region Network Receive
    public void HandleUseSkill(Creature owner, ResUseSkillToC skillPacket)
    {
        SkillData skillData;
        if (Managers.DataManager.SkillDict.TryGetValue(skillPacket.SkillInfo.SkillId, out skillData) == false)
            return;

        GameObject go = Managers.ObjectManager.FindById(skillPacket.SkillInfo.TargetId);
        if (go == null)
            owner.Machine.UseSkill(skillData, null, skillPacket.SkillInfo.PlayAnimName);
        else
        {
            Creature target = go.GetComponent<Creature>();
            owner.Machine.UseSkill(skillData, target, skillPacket.SkillInfo.PlayAnimName);
        }
    }

    public virtual void HandleModifyStat(StatInfo statInfo)
    {
        Stat.StatInfo.MergeFrom(statInfo);
        InvokeChangeHUD();
    }

    public virtual void HandleModifyOneStat(EStatType statType, float changedValue, float gapValue, EFontType fontType)
    {
        Stat.SetStat(statType, changedValue);

        switch (statType)
        {
            case EStatType.Hp:
                AddHUD();
                InvokeChangeHUD();
                FloatingTextController.RegisterOrSpawnText(gapValue, transform, fontType);
                break;
            default:
                break;
        }
    }

    public virtual void HandleDie(int killerId)
    {
        OnDie();
    }
    #endregion
}
