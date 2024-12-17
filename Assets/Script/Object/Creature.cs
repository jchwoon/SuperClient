using Data;
using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Google.Protobuf.Enum;
using System;

public class Creature : BaseObject
{
    CreatureHUD _creatureHUD;
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

    protected virtual void OnDie()
    {
        Machine.OnDie();
        ClearTarget();
        RemoveHUD();
    }

    protected virtual void OnHit()
    {

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
    //targetId는 누구에게서 Effect가 나갈건지
    public void SendUseSkill(int skillId, int skillTargetId, int skillLocationTargetId)
    {
        Vector2 joystickDir = Managers.GameManager.MoveInput;
        float rotY = joystickDir == Vector2.zero
            ? transform.rotation.eulerAngles.y
            : Utils.GetAngleFromDir(joystickDir);

        ReqUseSkillToS skillPacket = new ReqUseSkillToS();
        SkillInfo skillInfo = new SkillInfo()
        {
            SkillId = skillId,
            SkillLocationTargetId = skillLocationTargetId,
            SkillTargetId = skillTargetId,
            RotY = rotY
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

        GameObject go = Managers.ObjectManager.FindById(skillPacket.SkillInfo.SkillTargetId);
        if (go == null)
            owner.Machine?.UseSkill(skillData, null, skillPacket);
        else
        {
            Creature target = go.GetComponent<Creature>();
            owner.Machine?.UseSkill(skillData, target, skillPacket);
        }

        //해당 스킬에 대해한 파티클
        //해당 스킬 Hit에 대한 파티클
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
