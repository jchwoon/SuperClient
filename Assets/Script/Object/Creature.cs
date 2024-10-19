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
    public StatComponent Stat { get; protected set; }
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
        ClearTarget();
    }

    private void ClearTarget()
    {
        if (_isTargetted == true && Managers.ObjectManager.MyHero)
        {
            Managers.ObjectManager.MyHero.MyHeroStateMachine.Target = null;
        }
    }

    private void OnDie()
    {
        ClearTarget();
        _isTargetted = false;
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
    public void SendUseSkill(int skillId, int targetId = 0)
    {
        ReqUseSkillToS skillPacket = new ReqUseSkillToS();
        skillPacket.SkillId = skillId;
        skillPacket.TargetId = targetId;
        Managers.NetworkManager.Send(skillPacket);
    }
    #endregion

    #region Network Receive
    public void HandleUseSkill(Creature owner, ResUseSkillToC skillPacket)
    {
        SkillData skillData;
        if (Managers.DataManager.SkillDict.TryGetValue(skillPacket.SkillId, out skillData) == false)
            return;

        Creature target = Managers.ObjectManager.FindById(skillPacket.TargetId).GetComponent<Creature>();
        if (target != null)
            owner.Machine.UseSkill(skillData, target);
    }

    public virtual void HandleModifyStat(StatInfo statInfo)
    {
        Stat.StatInfo.MergeFrom(statInfo);
    }

    public virtual void HandleModifyOneStat(EStatType statType, float changedValue, float gapValue)
    {
        Stat.SetStat(statType, changedValue);

        if (IsTargetted == true)
        {
            switch (statType)
            {
                case EStatType.Hp:
                    Managers.EventBus.InvokeEvent(Enums.EventType.ChangeHUDInfo);
                    break;
                default:
                    break;
            }
        }
    }

    public virtual void HandleDie(int killerId)
    {
        Machine.OnDie();
        OnDie();
    }
    #endregion
}
