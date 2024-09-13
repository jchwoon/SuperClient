using Google.Protobuf.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HeroState
{
    public class MoveState : BaseState
    {
        Vector3 _posInput = Vector3.zero;
        Hero _hero;
        public MoveState(HeroStateMachine heroMachine) : base(heroMachine)
        {
        }

        public override void Exit()
        {
            base.Exit();
        }
        public override void Enter()
        {
            base.Enter();

            _hero = _heroMachine.Hero;
        }
        public override void Update()
        {
            base.Update();
            _posInput = _heroMachine.PosInput;
            if ((_posInput - _hero.transform.position).sqrMagnitude <= 0.01f)
            {
                _heroMachine.ChangeState(_heroMachine.IdleState);
                return;
            }


            SetAnimParameter(_hero.AnimData.MoveSpeedHash, _heroMachine.InputSpeed);
            MoveToMoveDir();
            RotateToMoveDir();
        }

        public override ECreatureState GetCreatureState()
        {
            return ECreatureState.Move;
        }

        private void MoveToMoveDir()
        {
            _hero.transform.position = Vector3.MoveTowards(_hero.transform.position, _posInput, _heroMachine.InputSpeed * Time.deltaTime);
        }

        private void RotateToMoveDir()
        {
            Vector3 targetDir = _posInput - _hero.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDir);
            _hero.transform.rotation = Quaternion.Slerp(_hero.transform.rotation, targetRotation, 10 * Time.deltaTime);
        }
    }

}
