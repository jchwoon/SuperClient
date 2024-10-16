using Google.Protobuf.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CreatureState
{
    public class MoveState : BaseState
    {
        Vector3 _posInput = Vector3.zero;
        public MoveState(CreatureMachine creatureMachine) : base(creatureMachine)
        {
        }
        public override void Exit()
        {
            base.Exit();
            _machine.SetAnimParameter(_owner, _owner.AnimData.MoveHash, false);
        }
        public override void Enter()
        {
            base.Enter();
            _machine.SetAnimParameter(_owner, _owner.AnimData.MoveHash, true);
        }
        public override void Update()
        {
            base.Update();
            if (_machine.PosInput.HasValue == false)
                return;

            _posInput = _machine.PosInput.Value;
            if ((_posInput - _owner.transform.position).sqrMagnitude <= 0.001f)
            {
                _machine.ChangeState(_machine.IdleState);
                return;
            }


            MoveToMoveDir();
            RotateToMoveDir();
        }

        public override ECreatureState GetCreatureState()
        {
            return ECreatureState.Move;
        }

        private void MoveToMoveDir()
        {
            float speed = _machine.ChaseMode == EMoveType.Chase ? _machine.Owner.StatInfo.ChaseSpeed :_machine.Owner.StatInfo.MoveSpeed;
            _owner.transform.position = Vector3.MoveTowards(_owner.transform.position, _posInput, speed * Time.deltaTime);
        }

        private void RotateToMoveDir()
        {
            Vector3 targetDir = _posInput - _owner.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDir);
            _owner.transform.rotation = Quaternion.Slerp(_owner.transform.rotation, targetRotation, 10 * Time.deltaTime);
        }
    }
}