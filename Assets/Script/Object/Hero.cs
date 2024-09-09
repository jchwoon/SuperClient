using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Creature
{
    protected HeroStateMachine _heroMachine;
    public HeroStateMachine HeroStateMachine
    {
        get { return _heroMachine; }
    }

    protected override void Awake()
    {
        base.Awake();

        _heroMachine = new HeroStateMachine(this);
        _heroMachine.ChangeState(_heroMachine.IdleState);

    }
    protected override void Update()
    {
        base.Update();

        _heroMachine.Update();
    }
}
