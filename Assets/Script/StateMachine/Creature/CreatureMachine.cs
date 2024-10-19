using CreatureState;
using Data;
using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CreatureMachine : StateMachine
{
    public virtual IdleState IdleState { get; set; }
    public virtual MoveState MoveState { get; set; }
    public virtual SkillState SkillState { get; set; }
    public EMoveType ChaseMode { get; protected set; }
    public Creature Owner { get; set; }
    public int? CurrentActiveSkillHash { get; set; }

    public CreatureMachine (Creature creature)
    {
        Owner = creature;
        SetState();
        ChangeState(IdleState);
    }
    private void SetState()
    {
        IdleState = new IdleState(this);
        MoveState = new MoveState(this);
        SkillState = new SkillState(this);
    }

    public override void UseSkill(SkillData skillData, Creature target)
    {
        if (skillData != null)
            Owner.Animator.Play(skillData.AnimName);
        if (target != null)
            Owner.transform.LookAt(target.transform);
        CreatureState = ECreatureState.Skill;
    }
    public override void OnDie()
    {
        CurrentState = null;
        SetAnimParameter(Owner, Owner.AnimData.DieHash);
        CreatureState = ECreatureState.Die;
    }

    public override void UpdatePosInput(MoveToC movePacket)
    {
        base.UpdatePosInput(movePacket);
        ChangeState(MoveState);
        ChaseMode = movePacket.MoveType;
        CreatureState = ECreatureState.Move;
    }
}
