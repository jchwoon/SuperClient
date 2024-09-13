using Google.Protobuf.Enum;
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
        }
        public override void Enter()
        {
            base.Enter();

            SetAnimParameter(_heroMachine.MyHero.AnimData.MoveSpeedHash, 0);
        }

        public override void Update()
        {
            base.Update();
            if (_heroMachine.MoveInput != Vector2.zero)
            {

                _heroMachine.ChangeState(_heroMachine.MoveState);
            }
        }

        public override ECreatureState GetCreatureState()
        {
            return ECreatureState.Idle;
        }
    }

}
