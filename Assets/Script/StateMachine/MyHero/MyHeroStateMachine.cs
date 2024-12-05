using Data;
using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using MyHeroState;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class MyHeroStateMachine : StateMachine
{
    public Creature Target { get; private set; }
    public IdleState IdleState { get; set; }
    public MoveState MoveState { get; set; }
    public SkillState SkillState { get; set; }
    public Vector2 MoveInput { get; private set; } = Vector2.zero;
    public MoveToS MovePacket { get; set; }
    public MyHero Owner { get; set; }
    //스킬 요청에 대한 응답을 대기중인지
    public bool isWaitSkillRes { get; set; } = false;
    public MyHeroStateMachine(MyHero myHero)
    {
        MovePacket = new MoveToS() { PosInfo = new PosInfo()};
        Owner = myHero;
        SetState();
        ChangeState(IdleState);
        Managers.GameManager.OnJoystickChanged += UpdateMoveInput;
    }

    public override void Update()
    {
        CheckAndSetIState();
        StateUpdate();
        FindAndSetTarget();
    }

    private void StateUpdate()
    {
        if (CurrentState == null)
            return;

        if (CreatureState == ECreatureState.Die)
            return;

        CurrentState.Update();
    }

    private void FindAndSetTarget()
    {
        Creature creature = FindTarget();

        //방금 찾은 Target과 이전의 Target이 다를경우
        //이전 Target을 Clear해주고 새로 찾은 Target을 OnTargetted
        if (creature != null && Target != creature)
        {
            Target?.ClearTarget();
            creature.OnTargetted();
        }
        Target = creature;
    }

    public Creature FindTarget()
    {
        if (CreatureState == ECreatureState.Die)
            return null;

        List<Creature> creatures = Managers.ObjectManager.GetAllCreatures();
        Creature target = null;
        float closestDist = float.MaxValue;
        foreach(Creature creature in creatures)
        {
            //if (creature.Machine.CurrentState == null) continue;
            float dist = (creature.gameObject.transform.position - Owner.transform.position).sqrMagnitude;
            if (dist < closestDist)
            {
                closestDist = dist;
                target = creature;
            }
        }

        return target;
    }

    private void UpdateMoveInput(Vector2 moveInput)
    {
        MoveInput = moveInput;
    }

    //기본공격 포함
    public override void UseSkill(SkillData skillData, Creature target, string playAnimName)
    {
        if (skillData == null)
            return;

        BaseSkill skill = Owner.SkillComponent.GetSkillById(skillData.TemplateId);
        if (target != null)
            Owner.transform.LookAt(target.transform);
        skill.UseSkill(playAnimName);
        ChangeState(SkillState);
    }

    public void OnAttack()
    {
        int normalSkillId = Owner.SkillComponent.NormalSkillId;
        Owner.SendUseSkill(normalSkillId, Target == null ? 0 : Target.ObjectId);
    }

    public override void OnDie()
    {
        SetAnimParameter(Owner, Owner.AnimData.DieHash);
        CreatureState = ECreatureState.Die;
        ChangeState(null);
    }
    public override void OnRevival()
    {
        base.OnRevival();
        ChangeState(IdleState);
        SetAnimParameter(Owner, Owner.AnimData.RevivalHash);
    }

    public void CheckAndSetIState()
    {
        //우선순위 1.Die, 2.스킬을 사용중인 상태, 3.움직이는 상태, 4.아이들 상태
        if (CreatureState == ECreatureState.Die)
            return;

        if (Owner.SkillComponent.isUsingSkill == true)
            return;

        if (MoveInput != Vector2.zero)
        {
            ChangeState(MoveState);
            return;
        }

        ChangeState(IdleState);
    }

    private void SetState()
    {
        IdleState = new IdleState(this);
        MoveState = new MoveState(this);
        SkillState = new SkillState(this);
    }
}
