using CreatureState;
using Google.Protobuf.Enum;
using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeroStateMachine : CreatureMachine
{
    public HeroIdleState HeroIdleState { get; set; }
    public HeroMoveState HeroMoveState { get; set; }

    public HeroStateMachine(Hero hero) : base(hero)
    {
        Owner = hero;
        SetState();
        ChangeState(HeroIdleState);
    }


    private void SetState()
    {
        HeroIdleState = new HeroIdleState(this);
        HeroMoveState = new HeroMoveState(this);
    }
}
