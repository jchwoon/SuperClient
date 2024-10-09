using CreatureState;
using Google.Protobuf.Enum;
using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeroStateMachine : CreatureMachine
{
    public override IdleState IdleState { get; set; }
    public override MoveState MoveState { get; set; }

    public HeroStateMachine(Creature creature) : base(creature)
    {
        Owner = creature;
        SetState();
    }


    private void SetState()
    {
        IdleState = new HeroIdleState(this);
        MoveState = new HeroMoveState(this);
    }
}
