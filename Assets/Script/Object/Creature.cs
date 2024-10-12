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
    private bool _isTargetted;
    public bool IsTargetted
    {
        get { return _isTargetted; }
        set { _isTargetted = value; }
    }

    public GrowthComponent GrowthInfo { get; protected set; } 
    public StatComponent Stat { get; protected set; }
    public Animator Animator { get; private set; }
    public AnimationData AnimData { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Animator = transform.GetComponent<Animator>();
        AnimData = new AnimationData();
        Stat = new StatComponent();
        GrowthInfo = new GrowthComponent();
        if (isMachineInit == false)
        {
            Machine = new CreatureMachine(this);
            isMachineInit = true;
        }
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
    public void ReceiveResUseSkill(Creature owner, ResUseSkillToC skillPacket)
    {
        SkillData skillData;
        if (Managers.DataManager.SkillDict.TryGetValue(skillPacket.SkillId, out skillData) == false)
            return;

        Creature target = Managers.ObjectManager.FindById(skillPacket.TargetId).GetComponent<Creature>();
        if (target != null)
            owner.Machine.UseSkill(skillData, target);
    }
    #endregion
}
