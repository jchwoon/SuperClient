using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using MyHeroState;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyHeroStateMachine : StateMachine
{
    public IdleState IdleState { get; set; }
    public MoveState MoveState { get; set; }
    public MyHero MyHero { get; private set; }
    public Vector2 MoveInput { get; set; } = Vector2.zero;
    public float MoveRatio { get; private set; } = 0.2f;
    public MoveToS MovePacket { get; set; }
    public MyHeroStateMachine(MyHero myHero)
    {
        MovePacket = new MoveToS() { PosInfo = new PosInfo()};
        MyHero = myHero;
        SetState();
        Managers.GameManager.OnJoystickChanged += UpdateMoveInput;
    }

    private void SetState()
    {
        IdleState = new IdleState(this);
        MoveState = new MoveState(this);
    }

    private void UpdateMoveInput(Vector2 moveInput)
    {
        MoveInput = moveInput;
    }
}
