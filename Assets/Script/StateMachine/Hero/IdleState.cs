using Google.Protobuf.Enum;
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
        }
        public override void Enter()
        {
            base.Enter();
            SetAnimParameter(_heroMachine.Hero.AnimData.MoveSpeedHash, 0);
        }

        public override void Update()
        {
            base.Update();
            if (_heroMachine.PosInput.HasValue == false)
                return;

            if ((_heroMachine.Hero.transform.position - _heroMachine.PosInput.Value).sqrMagnitude > 0.001f)
            {
                _heroMachine.ChangeState(_heroMachine.MoveState);
                return;
            }

        }

        public override ECreatureState GetCreatureState()
        {
            return ECreatureState.Idle;
        }
    }
}

