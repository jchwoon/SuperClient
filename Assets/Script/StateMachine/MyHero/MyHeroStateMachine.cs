using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using MyHeroState;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MyHeroStateMachine : StateMachine
{
    public IdleState IdleState { get; set; }
    public MoveState MoveState { get; set; }
    public AttackState AttackState { get; set; }
    public MyHero MyHero { get; private set; }
    public Vector2 MoveInput { get; set; } = Vector2.zero;
    public float MoveRatio { get; private set; } = 0.2f;
    public MoveToS MovePacket { get; set; }
    public Creature Target { get; set; }
    public bool Attacking { get; set; }
    public MyHeroStateMachine(MyHero myHero)
    {
        MovePacket = new MoveToS() { PosInfo = new PosInfo()};
        MyHero = myHero;
        SetState();
        Managers.GameManager.OnJoystickChanged += UpdateMoveInput;
    }

    public void OnAttack()
    {
        if (CurrentState == AttackState)
        {
            AttackState.StopComboExitRoutine();
            SetAnimParameter(MyHero.AnimData.AttackComboHash, true);
        }

        Target = FindTarget();
        if (Target != null)
            MyHero.transform.LookAt(Target.transform);
        ChangeState(AttackState);
    }

    public Creature FindTarget()
    {
        if (Target != null)
        {
            float targetDist = Vector3.Distance(Target.transform.position, MyHero.transform.position);
            if (targetDist > 4f)
                Target = null;
            else
                return Target;
        }
        Creature target = null;
        int mask = 1 << (int)Enums.Layers.Monster;
        Collider[] colliders = Physics.OverlapSphere(MyHero.transform.position, 4f, mask);
        float closestDist = float.MaxValue;
        foreach(Collider collider in colliders)
        {
            float dist = (collider.gameObject.transform.position - MyHero.transform.position).sqrMagnitude;
            if (dist < closestDist)
                target = collider.gameObject.GetComponent<Creature>();
        }
        return target;
    }

    public override void ChangeState(IState changeState)
    {
        if (Attacking == true) return;
        base.ChangeState(changeState);
    }

    private void SetState()
    {
        IdleState = new IdleState(this);
        MoveState = new MoveState(this);
        AttackState = new AttackState(this);
    }

    private void UpdateMoveInput(Vector2 moveInput)
    {
        MoveInput = moveInput;
    }
    #region AnimParamGetSet
    public bool GetAnimParameter(int hashId)
    {
        return MyHero.Animator.GetBool(hashId);
    }

    public void SetAnimParameter(int hashId, bool value)
    {
        MyHero.Animator.SetBool(hashId, value);
    }

    public void SetAnimParameter(int hashId, float value)
    {
        MyHero.Animator.SetFloat(hashId, value);
    }
    public void SetAnimParameter(int hashId, int value)
    {
        MyHero.Animator.SetInteger(hashId, value);
    }
    public void SetAnimParameter(int hashId)
    {
        MyHero.Animator.SetTrigger(hashId);
    }
    #endregion

}
