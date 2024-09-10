using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroState
{
    public class BaseState : IState
    {
        HeroStateMachine _heroMachine;
        public BaseState(HeroStateMachine heroMachine)
        {
            _heroMachine = heroMachine;
        }
        public virtual void Enter()
        {
            
        }

        public virtual void Exit()
        {
            
        }

        public virtual void Update()
        {
            
        }
    }

}
