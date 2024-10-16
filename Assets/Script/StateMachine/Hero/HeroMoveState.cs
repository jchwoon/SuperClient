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
    }
    public override void Enter()
    {
        Creature owner = _machine.Owner;
        _machine.SetAnimParameter(owner, owner.AnimData.MoveSpeedHash, _machine.Owner.StatInfo.MoveSpeed);
    }
    public override void Update()
    {
        base.Update();
    }
}
