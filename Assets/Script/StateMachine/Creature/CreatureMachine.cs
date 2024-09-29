using CreatureState;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureMachine : StateMachine
{
    public IdleState IdleState { get; set; }
    public MoveState MoveState { get; set; }
    public Creature Creature { get; private set; }
    public CreatureMachine (Creature creature)
    {
        Creature = creature;
        SetState();
    }
    private void SetState()
    {
        IdleState = new IdleState(this);
        MoveState = new MoveState(this);
    }
}
