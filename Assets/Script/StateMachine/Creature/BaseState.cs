using Google.Protobuf.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CreatureState
{
    public class BaseState : IState
    {
        protected CreatureMachine _machine;
        protected Creature _owner;
        public BaseState(CreatureMachine creatureMachine)
        {
            _machine = creatureMachine;
            _owner = _machine.Owner;
        }

        public virtual void Enter()
        {
        }

        public virtual void Exit()
        {
        }

        public virtual ECreatureState GetCreatureState()
        {
            return ECreatureState.Idle;    
        }

        public virtual void Update()
        {
        }
    }
}