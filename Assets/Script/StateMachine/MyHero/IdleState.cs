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

            _heroMachine.SetAnimParameter(_owner, _owner.AnimData.MoveSpeedHash, 0.0f);
        }

        public override void Update()
        {
            base.Update();
            if (CheckChangeState() == true)
                return;
        }

        public override ECreatureState GetCreatureState()
        {
            return ECreatureState.Idle;
        }

        private bool CheckChangeState()
        {
            if (_heroMachine.MoveInput != Vector2.zero)
            {
                _heroMachine.ChangeState(_heroMachine.MoveState);
                return true;    
            }
            else if (_heroMachine.Attacking == true)
            {
                //Todo : 거리 비교해서 범위 안이면 skill 밖이면 Move
                _heroMachine.ChangeState(_heroMachine.MoveState);
                return true;
            }

            return false;
        }
    }

}
