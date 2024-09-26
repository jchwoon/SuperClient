using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyHeroState
{
    public class MoveState : BaseState
    {
        Vector2 _moveInput = Vector2.zero;
        MyHero _myHero;
        Coroutine sendRoutine;

        public MoveState(MyHeroStateMachine heroMachine) : base(heroMachine)
        {
        }

        public override void Exit()
        {
            base.Exit();

            CoroutineHelper.Instance.StopHelperCoroutine(sendRoutine);
        }
        public override void Enter()
        {
            base.Enter();

            _myHero = _heroMachine.MyHero;
            _moveInput = _heroMachine.MoveInput;
            sendRoutine = CoroutineHelper.Instance.StartHelperCoroutine(SendMyPos());
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
            RotateToMoveDir();
        }

        public override ECreatureState GetCreatureState()
        {
            return ECreatureState.Move;
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


            Vector3 pos = _myHero.transform.position + moveDir;
            bool canGo = Managers.MapManager.CanGo(pos.z, pos.x);
            if (canGo == false)
                return;
            _myHero.Agent.Move(moveDir);
        }

        private void RotateToMoveDir()
        {
            Vector3 targetDir = new Vector3(_moveInput.x, 0, _moveInput.y);
            Quaternion targetRotation = Quaternion.LookRotation(targetDir);
            _myHero.transform.rotation = Quaternion.Slerp(_myHero.transform.rotation, targetRotation, 10 * Time.deltaTime);
        }

        private float GetModifiedSpeed()
        {
            return 20 * _heroMachine.MoveRatio;
        }

        IEnumerator SendMyPos()
        {
            while (true)
            {
                _heroMachine.MovePacket.PosInfo.PosX = _myHero.transform.position.x;
                _heroMachine.MovePacket.PosInfo.PosY = _myHero.transform.position.y;
                _heroMachine.MovePacket.PosInfo.PosZ = _myHero.transform.position.z;
                _heroMachine.MovePacket.PosInfo.RotY = _myHero.transform.eulerAngles.y;
                _heroMachine.MovePacket.PosInfo.Speed = _moveInput.magnitude * GetModifiedSpeed();
                Managers.NetworkManager.Send(_heroMachine.MovePacket);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
