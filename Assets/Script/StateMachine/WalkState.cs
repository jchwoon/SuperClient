using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyHeroState
{
    public class WalkState : BaseState
    {
        Vector2 _moveInput = Vector2.zero;
        MyHero _myHero;

        public WalkState(MyHeroStateMachine heroMachine) : base(heroMachine)
        {
        }

        public override void Exit()
        {
            base.Exit();

            SetAnimParameter(_heroMachine.MyHero.AnimData.WalkHash, false);
        }
        public override void Enter()
        {
            base.Enter();

            _myHero = _heroMachine.MyHero;

            SetAnimParameter(_heroMachine.MyHero.AnimData.WalkHash, true);
        }

        public override void Update()
        {
            base.Update();
            _moveInput = _heroMachine.MoveInput;
            if (_heroMachine.MoveInput == Vector2.zero)
            {
                _heroMachine.ChangeState(_heroMachine.IdleState);
                return;
            }

            SetAnimParameter(_myHero.AnimData.MoveSpeedHash, _moveInput.magnitude * GetModifiedSpeed());
            MoveToMoveDir();
            RotateToMoveDir(_moveInput);
        }

        private void MoveToMoveDir()
        {
            Vector3 moveDir;
            //최소 속도제한
            if (_moveInput.magnitude < 0.3)
                moveDir = new Vector3(_moveInput.normalized.x * 0.3f, 0, _moveInput.normalized.y * 0.3f);
            else
                moveDir = new Vector3(_moveInput.x, 0, _moveInput.y);

            moveDir *= (GetModifiedSpeed() * Time.deltaTime);


            _myHero.transform.position += moveDir;
        }

        private void RotateToMoveDir(Vector2 moveInput)
        {
            Vector3 targetDir = new Vector3(moveInput.x, 0, moveInput.y);
            Quaternion targetRotation = Quaternion.LookRotation(targetDir);
            _myHero.transform.rotation = Quaternion.Slerp(_myHero.transform.rotation, targetRotation, 10 * Time.deltaTime);
        }

        private float GetModifiedSpeed()
        {
            return _myHero.StatData.MoveSpeed * _heroMachine.MoveRatio;
        }
    }

}
