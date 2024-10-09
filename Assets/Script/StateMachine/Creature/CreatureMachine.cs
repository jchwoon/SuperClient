using CreatureState;
using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureMachine : StateMachine
{
    public virtual IdleState IdleState { get; set; }
    public virtual MoveState MoveState { get; set; }
    public Creature Owner { get; set; }
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
    }

    public override void UseSkill(SkillData skillData, Creature target)
    {
        Owner.Animator.Play(skillData.AnimName);

        if (target != null)
            Owner.transform.LookAt(target.transform);
    }
}
