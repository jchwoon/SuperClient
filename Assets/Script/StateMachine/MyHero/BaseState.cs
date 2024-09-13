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

        protected virtual void SetAnimParameter(int hashId, bool value)
        {
            _heroMachine.MyHero.Animator.SetBool(hashId, value);
        }

        protected virtual void SetAnimParameter(int hashId, float value)
        {
            _heroMachine.MyHero.Animator.SetFloat(hashId, value);
        }
    }
}

