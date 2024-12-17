using CreatureState;
using Data;
using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public override void UseSkill(SkillData skillData, Creature target, ResUseSkillToC skillPacket)
    {
        if (skillData == null || target == null)
            return;

        Owner.transform.LookAt(target.transform);
        if (skillData.IsMoveSkill)
        {
            PosInfo posInfo = skillPacket.PosInfo;
            Vector3 destPos = new Vector3(posInfo.PosX, posInfo.PosY, posInfo.PosZ);
            CoroutineHelper.Instance.StartHelperCoroutine(CoMoveFromSkillData(Owner, skillData, destPos));
        }
        Owner.Animator.Play(skillPacket.SkillInfo.PlayAnimName);
        //스킬을 썻을 때 위치가 변하는 스킬이면 위치 동기화 처리
        CreatureState = ECreatureState.Skill;
    }
    public override void OnDie()
    {
        SetAnimParameter(Owner, Owner.AnimData.DieHash);
        CreatureState = ECreatureState.Die;
    }

    public override void OnDamage()
    {
        Owner.Animator.Play(Owner.AnimData.HitHash);
        CreatureState = ECreatureState.Hit;
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
