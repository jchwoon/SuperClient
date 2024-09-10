using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyHeroState
{
    public class IdleState : BaseState
    {
        public IdleState(MyHeroStateMachine heroMachine) : base(heroMachine)
        {
        }

        public override void Exit()
        {
            base.Exit();

            //SetAnimParameter(_heroMachine.MyHero.AnimData.IdleHash, false);
        }
        public override void Enter()
        {
            base.Enter();
            SetAnimParameter(_heroMachine.MyHero.AnimData.MoveSpeedHash, 0);
            //SetAnimParameter(_heroMachine.MyHero.AnimData.IdleHash, true);
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
