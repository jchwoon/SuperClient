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

            _heroMachine.SetAnimParameter(_owner, _owner.AnimData.MoveSpeedHash, _moveInput.magnitude * GetModifiedSpeed());
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


            Vector3 pos = _owner.transform.position + moveDir;
            bool canGo = Managers.MapManager.CanGo(pos.z, pos.x);
            if (canGo == false)
                return;
            _owner.Agent.Move(moveDir);
        }

        private void RotateToMoveDir()
        {
            Vector3 targetDir = new Vector3(_moveInput.x, 0, _moveInput.y);
            Quaternion targetRotation = Quaternion.LookRotation(targetDir);
            _owner.transform.rotation = Quaternion.Slerp(_owner.transform.rotation, targetRotation, 10 * Time.deltaTime);
        }

        private float GetModifiedSpeed()
        {
            return 20 * _heroMachine.MoveRatio;
        }

        IEnumerator SendMyPos()
        {
            while (true)
            {
                _heroMachine.MovePacket.PosInfo.PosX = _owner.transform.position.x;
                _heroMachine.MovePacket.PosInfo.PosY = _owner.transform.position.y;
                _heroMachine.MovePacket.PosInfo.PosZ = _owner.transform.position.z;
                _heroMachine.MovePacket.PosInfo.RotY = _owner.transform.eulerAngles.y;
                _heroMachine.MovePacket.PosInfo.Speed = _moveInput.magnitude * GetModifiedSpeed();
                Managers.NetworkManager.Send(_heroMachine.MovePacket);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
