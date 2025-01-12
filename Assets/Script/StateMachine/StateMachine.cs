using Data;
using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using Google.Protobuf.WellKnownTypes;
using MyHeroState;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

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

    protected virtual void ChangeState(IState changeState)
    {
        if (CurrentState == changeState) return;
        if (CurrentState != null)
        {
            CurrentState.Exit();
        }

        CurrentState = changeState;

        CurrentState?.Enter();
    }

    public virtual void Update()
    {
        if (CurrentState == null)
            return;

        if (CreatureState == ECreatureState.Die)
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

    public virtual void UseSkill(ActiveSkillData skillData, Creature target, ResUseSkillToC skillPacket)
    {
    }
    public virtual void OnDie()
    {

    }

    public virtual void OnDamage()
    {

    }

    public virtual void OnRevival()
    {
    }

    protected IEnumerator CoMoveFromSkillData(Creature owner, ActiveSkillData skillData, Vector3 destPos)
    {
        float duration = skillData.AnimTime * skillData.EffectDelayRatio;

        Vector3 startPos = owner.transform.position;
        owner.transform.LookAt(destPos);

        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            owner.transform.position = Vector3.Lerp(startPos, destPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        owner.transform.position = destPos;
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
