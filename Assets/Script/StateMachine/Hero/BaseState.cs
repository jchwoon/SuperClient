using Google.Protobuf.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroState
{
    public class BaseState : IState
    {
        protected HeroStateMachine _heroMachine;
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
        public virtual ECreatureState GetCreatureState()
        {
            return ECreatureState.Idle;
        }

        protected virtual void SetAnimParameter(int hashId, bool value)
        {
            _heroMachine.Hero.Animator.SetBool(hashId, value);
        }

        protected virtual void SetAnimParameter(int hashId, float value)
        {
            _heroMachine.Hero.Animator.SetFloat(hashId, value);
        }
    }

}
