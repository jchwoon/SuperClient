using Google.Protobuf.Enum;
using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    public void Exit();
    public void Enter();
    public void Update();
    public ECreatureState GetCreatureState();
}

public class StateMachine
{
    public IState CurrentState { get; private set; }
    protected ECreatureState CreatureState { get; private set; }
    public Vector3 PosInput { get; private set; } = Vector3.zero;
    public float InputSpeed { get; private set; }

    public virtual void ChangeState(IState changeState)
    {
        if (CurrentState != null)
        {
            CurrentState.Exit();
        }

        CurrentState = changeState;

        CurrentState.Enter();
    }

    public void Update()
    {
        if (CurrentState == null)
            return;

        CurrentState.Update();
    }

    public void UpdatePosInput(PosInfo pos)
    {
        PosInput = new Vector3(pos.PosX, pos.PosY, pos.PosZ);
        InputSpeed = pos.Speed;
    }
}
