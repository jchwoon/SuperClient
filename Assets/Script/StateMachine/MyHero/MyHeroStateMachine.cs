using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using MyHeroState;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class MyHeroStateMachine : StateMachine
{
    public IdleState IdleState { get; set; }
    public MoveState MoveState { get; set; }
    public SkillState SkillState { get; set; }
    public Vector2 MoveInput { get; set; } = Vector2.zero;
    public Vector3 DestPos { get; set; } = Vector3.zero;
    public float MoveRatio { get; private set; } = 0.2f;
    public MoveToS MovePacket { get; set; }
    public Creature Target { get; set; }
    public bool Attacking { get; set; } = false;
    //Target을 기준으로 움직일지 Input을 기준으로 움직일지
    public bool TargetMode { get; set; } = false;
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
        if (Attacking == false)
        {
            if (Target == null)
                Target = FindTarget();
            if (Target == null)
            {
                Managers.UIManager.ShowToasUI("주위에 지정할 타겟이 없습니다.");
                return;
            }
            TargetMode = true;
        }
        else
        {
            TargetMode = false;
        }

        ToggleAttacking();
    }

    public Creature FindTarget()
    {
        List<Creature> creatures = Managers.ObjectManager.GetAllCreatures();
        Creature target = null;
        float closestDist = float.MaxValue;
        foreach(Creature creature in creatures)
        {
            float dist = (creature.gameObject.transform.position - Owner.transform.position).sqrMagnitude;
            if (dist < closestDist)
                target = creature.gameObject.GetComponent<Creature>();
        }
        return target;
    }

    public override void ChangeState(IState changeState)
    {
        base.ChangeState(changeState);
    }

    public float GetDistToTarget()
    {
        if (Target == null)
            return 0;

        return (Target.transform.position - Owner.transform.position).magnitude;
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
        else if (Attacking == true)
            TargetMode = true;
    }

    private void ToggleAttacking()
    {
        Attacking = Attacking == true ? false : true;
    }
}
