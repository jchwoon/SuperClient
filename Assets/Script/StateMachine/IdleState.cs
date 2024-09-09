using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroState
{
    public class IdleState : BaseState
    {
        public IdleState(HeroStateMachine heroMachine) : base(heroMachine)
        {
        }

        public override void Exit()
        {
            base.Exit();

            StopAnim(_heroMachine.Hero.AnimData.IdleHash);
        }
        public override void Enter()
        {
            base.Enter();

            StartAnim(_heroMachine.Hero.AnimData.IdleHash);
        }

        public override void Update()
        {
            base.Update();
            if (_heroMachine.MoveInput != Vector2.zero)
            {

                _heroMachine.ChangeState(_heroMachine.WalkState);
            }
        }
    }

}
