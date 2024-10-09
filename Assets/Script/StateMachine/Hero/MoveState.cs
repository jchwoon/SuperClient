using CreatureState;
using Google.Protobuf.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeroMoveState : MoveState
{
    public HeroMoveState(HeroStateMachine heroMachine) : base(heroMachine)
    {
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
        _machine.SetAnimParameter(_owner, _owner.AnimData.MoveSpeedHash, _machine.InputSpeed);
    }
}
