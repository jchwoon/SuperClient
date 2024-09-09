using HeroState;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroStateMachine : StateMachine
{
    public Hero Hero { get; set; }
    public IdleState IdleState { get; set; }
    public WalkState WalkState { get; set; }
    public Vector2 MoveInput { get; set; } = Vector2.zero;
    public HeroStateMachine(Hero hero)
    {
        Hero = hero;
        SetState();
        Managers.GameManager.OnJoystickChanged += dd;
    }

    private void SetState()
    {
        IdleState = new IdleState(this);
        WalkState = new WalkState(this);
    }

    private void dd(Vector2 moveInput)
    {
        MoveInput = moveInput;
    }
}
