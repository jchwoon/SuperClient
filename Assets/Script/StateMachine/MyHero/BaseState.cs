using Google.Protobuf.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyHeroState
{
    public class BaseState : IState
    {
        protected MyHeroStateMachine _heroMachine;

        public BaseState(MyHeroStateMachine heroMachine)
        {
            _heroMachine = heroMachine;
        }
        public virtual void Exit()
        {

        }
        public virtual void Enter()
        {

        }

        public virtual void Update()
        {

        }
        public virtual ECreatureState GetCreatureState()
        {
            return ECreatureState.Idle;
        }
    }
}

