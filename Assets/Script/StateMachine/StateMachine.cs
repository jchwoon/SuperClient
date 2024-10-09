using Data;
using Google.Protobuf.Enum;
using Google.Protobuf.Struct;
using Google.Protobuf.WellKnownTypes;
using MyHeroState;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    public void Exit();
    public void Enter();
    public void Update();
    public ECreatureState GetCreatureState();
}

public class StateMachine
{
    public IState CurrentState { get; private set; }
    protected ECreatureState CreatureState { get; private set; }
    public Vector3? PosInput { get; private set; } = null;
    public float InputSpeed { get; private set; }

    public virtual void ChangeState(IState changeState)
    {
        if (CurrentState == changeState) return;
        if (CurrentState != null)
        {
            CurrentState.Exit();
        }

        CurrentState = changeState;

        CurrentState.Enter();
    }

    public void Update()
    {
        if (CurrentState == null)
            return;

        CurrentState.Update();
    }

    public void UpdatePosInput(PosInfo pos)
    {
        PosInput = new Vector3(pos.PosX, pos.PosY, pos.PosZ);
        InputSpeed = pos.Speed;
    }

    public virtual void FindTargetAndAttack()
    {

    }

    public virtual void UseSkill(SkillData skillData, Creature target)
    {
    }

    #region AnimParamGetSet
    public bool GetAnimParameter(Creature creature, int hashId)
    {
        return creature.Animator.GetBool(hashId);
    }

    public void SetAnimParameter(Creature creature, int hashId, bool value)
    {
        creature.Animator.SetBool(hashId, value);
    }

    public void SetAnimParameter(Creature creature, int hashId, float value)
    {
        creature.Animator.SetFloat(hashId, value);
    }
    public void SetAnimParameter(Creature creature, int hashId, int value)
    {
        creature.Animator.SetInteger(hashId, value);
    }
    public void SetAnimParameter(Creature creature, int hashId)
    {
        creature.Animator.SetTrigger(hashId);
    }

    public void SetAnimParameter(Creature creature, string hashName, bool value)
    {
        SetAnimParameter(creature, Animator.StringToHash(hashName), value);
    }

    public void SetAnimParameter(Creature creature, string hashName, float value)
    {
        SetAnimParameter(creature, Animator.StringToHash(hashName), value);
    }
    public void SetAnimParameter(Creature creature, string hashName, int value)
    {
        SetAnimParameter(creature, Animator.StringToHash(hashName), value);
    }
    public void SetAnimParameter(Creature creature, string hashName)
    {
        SetAnimParameter(creature, Animator.StringToHash(hashName));
    }
    #endregion
}
