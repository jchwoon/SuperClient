using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    public void Exit();
    public void Enter();
    public void Update();
}

public class StateMachine
{
    protected IState CurrentState { get; private set; }
    public void ChangeState(IState changeState)
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
}
