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
        public virtual void Exit()
        {

        }
        public virtual void Enter()
        {

        }

        public virtual void Update()
        {

        }

        protected virtual void StartAnim(int hashId)
        {
            _heroMachine.Hero.Animator.SetBool(hashId, true);
        }

        protected virtual void StopAnim(int hashId)
        {
            _heroMachine.Hero.Animator.SetBool(hashId, false);
        }
    }
}

