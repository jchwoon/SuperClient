using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Creature
{
    private CreatureMachine _creatureMachine;

    public CreatureMachine CreatureMachine
    {
        get { return _creatureMachine; }
    }

    protected override void Awake()
    {
        base.Awake();

        _creatureMachine = new CreatureMachine(this);
        Machine = _creatureMachine;
        Machine.ChangeState(_creatureMachine.IdleState);
    }
    protected override void Update()
    {
        base.Update();

    }
}
