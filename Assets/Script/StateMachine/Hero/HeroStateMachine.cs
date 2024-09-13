using Google.Protobuf.Enum;
using Google.Protobuf.Struct;
using HeroState;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeroStateMachine : StateMachine
{
    public IdleState IdleState { get; set; }
    public MoveState MoveState { get; set; }
    public Hero Hero { get; private set; }
    public Vector3 PosInput { get; private set; } = Vector3.zero;
    public float InputSpeed { get; private set; }
    public float MoveRatio { get; private set; } = 0.2f;
    public HeroStateMachine(Hero hero)
    {
        Hero = hero;
        SetState();
    }

    public void UpdatePosInput(PosInfo pos)
    {
        PosInput = new Vector3(pos.PosX, pos.PosY,pos.PosZ);
        InputSpeed = pos.Speed;
    }

    private void SetState()
    {
        IdleState = new IdleState(this);
        MoveState = new MoveState(this);
    }
}
