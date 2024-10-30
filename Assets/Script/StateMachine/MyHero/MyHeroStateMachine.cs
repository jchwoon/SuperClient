using Data;
using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using MyHeroState;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyHeroStateMachine : StateMachine
{
    private Creature _target;
    public Creature Target
    {
        get { return _target; }
        set
        {
            if (value == null)
            {
                if (_target != null)
                {
                    OffAttackMode();
                    _target.IsTargetted = false;
                }
            }
            else
            {
                if (_target != null)
                    _target.IsTargetted = false;
                value.IsTargetted = true;
            }
            _target = value;
        }
    }
    public IdleState IdleState { get; set; }
    public MoveState MoveState { get; set; }
    public SkillState SkillState { get; set; }
    public Vector2 MoveInput { get; set; } = Vector2.zero;
    public MoveToS MovePacket { get; set; }
    public MyHero Owner { get; set; }
    //���ø�尡 Ȱ��ȭ �Ǿ��ִ��� �ƴ���
    public bool AttackMode { get; set; } = false;
    //Target�� �������� �������� Input�� �������� ��������
    public bool TargetMode { get; set; } = false;
    //��ų ��û�� ���� ������ ���������
    public bool isWaitSkillRes { get; set; } = false;
    public MyHeroStateMachine(MyHero myHero)
    {
        MovePacket = new MoveToS() { PosInfo = new PosInfo()};
        Owner = myHero;
        SetState();
        ChangeState(IdleState);
        Managers.GameManager.OnJoystickChanged += UpdateMoveInput;
    }
    public override void FindTargetAndAttack()
    {
        if (Target == null)
            Target = FindTarget();
        if (Target == null)
        {
            Managers.UIManager.ShowToasUI("������ ������ Ÿ���� �����ϴ�.");
            return;
        }
        TargetMode = true;
        ChangeAttackMode(true);
    }

    public void OffAttackMode()
    {
        TargetMode = false;
        ChangeAttackMode(false);
    }

    public Creature FindTarget()
    {
        List<Creature> creatures = Managers.ObjectManager.GetAllCreatures();
        Creature target = null;
        float closestDist = float.MaxValue;
        foreach(Creature creature in creatures)
        {
            if (creature.Machine.CreatureState == ECreatureState.Die) continue;
            float dist = (creature.gameObject.transform.position - Owner.transform.position).sqrMagnitude;
            if (dist < closestDist)
            {
                closestDist = dist;
                target = creature.gameObject.GetComponent<Creature>();
            }
        }
        return target;
    }

    public override void UseSkill(SkillData skillData, Creature target, string playAnimName)
    {
        if (skillData == null || target == null)
            return;

        BaseSkill skill = Owner.SkillComponent.GetSkillById(skillData.SkillId);
        skill.UseSkill(playAnimName);
        ChangeState(SkillState);
        Owner.transform.LookAt(target.transform);
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

    private void SetState()
    {
        IdleState = new IdleState(this);
        MoveState = new MoveState(this);
        SkillState = new SkillState(this);
    }

    private void UpdateMoveInput(Vector2 moveInput)
    {
        MoveInput = moveInput;
        if (moveInput != Vector2.zero)
            TargetMode = false;
        else if (AttackMode == true)
            TargetMode = true;
    }

    private void ChangeAttackMode(bool mode)
    {
        AttackMode = mode;
        Managers.EventBus.InvokeEvent(Enums.EventType.ChangeAttackMode);
    }
}
