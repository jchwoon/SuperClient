using Google.Protobuf.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CreatureState
{
    public class IdleState : BaseState
    {
        public IdleState(CreatureMachine creatureMachine) : base(creatureMachine)
        {
        }
        public override void Exit()
        {
            base.Exit();
        }
        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
            if (_creatureMachine.PosInput.HasValue == false)
                return;

            if ((_creatureMachine.Creature.transform.position - _creatureMachine.PosInput.Value).sqrMagnitude > 0.001f)
            {
                _creatureMachine.ChangeState(_creatureMachine.MoveState);
                return;
            }

        }

        public override ECreatureState GetCreatureState()
        {
            return ECreatureState.Idle;
        }
    }
}
