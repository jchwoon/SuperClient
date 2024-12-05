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
            _heroMachine.CreatureState = ECreatureState.Idle;
        }

        public override void Update()
        {
            base.Update();

        }

        public override ECreatureState GetCreatureState()
        {
            return ECreatureState.Idle;
        }
    }

}
