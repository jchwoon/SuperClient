using CreatureState;
using Data;
using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class CreatureMachine : StateMachine
{
    public IdleState IdleState { get; set; }
    public MoveState MoveState { get; set; }
    public virtual SkillState SkillState { get; set; }
    public EMoveType ChaseMode { get; protected set; }
    public Creature Owner { get; set; }

    public CreatureMachine (Creature creature)
    {
        Owner = creature;
        SetState();
        ChangeState(IdleState);
    }

    public override void Update()
    {
        base.Update();
        CheckAndSetState();
    }

    public override void UseSkill(SkillData skillData, Creature target, string playAnimName)
    {
        if (skillData == null || target == null)
            return;

        Owner.transform.LookAt(target.transform);
        Owner.Animator.Play(playAnimName);
        CreatureState = ECreatureState.Skill;
    }
    public override void OnDie()
    {
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

    public void CheckAndSetState()
    {
        if (PosInput.HasValue == false)
            return;

        if ((Owner.transform.position - PosInput.Value).sqrMagnitude <= 0.001f)
            ChangeState(IdleState);
    }

    private void SetState()
    {
        IdleState = new IdleState(this);
        MoveState = new MoveState(this);
        SkillState = new SkillState(this);
    }
}
