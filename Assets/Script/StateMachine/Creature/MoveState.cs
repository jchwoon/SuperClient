using Google.Protobuf.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CreatureState
{
    public class MoveState : BaseState
    {
        Vector3 _posInput = Vector3.zero;
        Creature _creature;
        public MoveState(CreatureMachine creatureMachine) : base(creatureMachine)
        {
        }
        public override void Exit()
        {
            base.Exit();
        }
        public override void Enter()
        {
            base.Enter();
            _creature = _creatureMachine.Creature;
        }
        public override void Update()
        {
            base.Update();
            if (_creatureMachine.PosInput.HasValue == false)
                return;

            _posInput = _creatureMachine.PosInput.Value;
            if ((_posInput - _creature.transform.position).sqrMagnitude <= 0.001f)
            {
                _creatureMachine.ChangeState(_creatureMachine.IdleState);
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
            _creature.transform.position = Vector3.MoveTowards(_creature.transform.position, _posInput, 3 * Time.deltaTime);
        }

        private void RotateToMoveDir()
        {
            Vector3 targetDir = _posInput - _creature.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDir);
            _creature.transform.rotation = Quaternion.Slerp(_creature.transform.rotation, targetRotation, 10 * Time.deltaTime);
        }
    }
}