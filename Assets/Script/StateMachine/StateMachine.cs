using Data;
using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
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
    public IState CurrentState { get; protected set; }
    public ECreatureState CreatureState { get; set; }
    public Vector3? PosInput { get; private set; } = null;

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

    public virtual void UpdatePosInput(MoveToC movePacket)
    {
        PosInput = new Vector3(movePacket.PosInfo.PosX, movePacket.PosInfo.PosY, movePacket.PosInfo.PosZ);
    }

    public virtual void FindTargetAndAttack()
    {

    }

    public virtual void UseSkill(SkillData skillData, Creature target)
    {
    }
    public virtual void OnDie()
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
