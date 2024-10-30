using CreatureState;
using Google.Protobuf.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroIdleState : IdleState
{
    public HeroIdleState(HeroStateMachine heroMachine) : base(heroMachine)
    {
    }

    public override void Exit()
    {

    }
    public override void Enter()
    {
        _machine.SetAnimParameter(_owner, _owner.AnimData.MoveSpeedHash, 0.0f);
    }

    public override void Update()
    {
        base.Update();
    }

    public override ECreatureState GetCreatureState()
    {
        return ECreatureState.Idle;
    }
}

