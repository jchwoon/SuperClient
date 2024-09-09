using MyHeroState;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyHeroStateMachine : StateMachine
{
    public IdleState IdleState { get; set; }
    public WalkState WalkState { get; set; }
    public MyHero MyHero { get; private set; }
    public Vector2 MoveInput { get; set; } = Vector2.zero;
    public float MoveRatio { get; private set; } = 0.2f;
    public MyHeroStateMachine(MyHero myHero)
    {
        MyHero = myHero;
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
