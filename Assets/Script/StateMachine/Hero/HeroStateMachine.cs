using HeroState;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroStateMachine : StateMachine
{
    public IdleState IdleState { get; set; }
    public MoveState WalkState { get; set; }
    public Hero Hero { get; private set; }
    public HeroStateMachine(Hero hero)
    {
        Hero = hero;
        SetState();
    }

    private void SetState()
    {
        IdleState = new IdleState(this);
        WalkState = new MoveState(this);
    }
}
