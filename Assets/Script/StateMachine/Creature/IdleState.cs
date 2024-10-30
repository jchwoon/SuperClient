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
            _machine.SetAnimParameter(_owner, _owner.AnimData.IdleHash, false);
        }
        public override void Enter()
        {
            base.Enter();
            _machine.SetAnimParameter(_owner, _owner.AnimData.IdleHash, true);
            _machine.CreatureState = ECreatureState.Idle;
        }

        public override void Update()
        {
            base.Update();
            if (_machine.PosInput.HasValue == false)
                return;

            if ((_owner.transform.position - _machine.PosInput.Value).sqrMagnitude > 0.001f)
            {
                _machine.ChangeState(_machine.MoveState);
                return;
            }
        }

        public override ECreatureState GetCreatureState()
        {
            return ECreatureState.Idle;
        }
    }
}
