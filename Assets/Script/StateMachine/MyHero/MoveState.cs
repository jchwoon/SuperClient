using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.PlayerSettings;
using static UnityEngine.UI.GridLayoutGroup;

namespace MyHeroState
{
    public class MoveState : BaseState
    {
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
            sendRoutine = CoroutineHelper.Instance.StartHelperCoroutine(SendMyPos());
            MyHero owner = _heroMachine.Owner;
            _heroMachine.SetAnimParameter(owner, owner.AnimData.MoveSpeedHash, _heroMachine.GetModifiedSpeed());
        }

        public override void Update()
        {
            base.Update();

            if (CheckChangeState() == true)
                return;

            MoveToInputDirOrTarget();
        }

        public override ECreatureState GetCreatureState()
        {
            return ECreatureState.Move;
        }

        private void MoveToInputDirOrTarget()
        {
            if (_heroMachine.TargetMode == true)
                MoveToTarget();
            else
                MoveToInputDir();

        }
        private void MoveToTarget()
        {
            if (_heroMachine.Target == null)
                return;
            Vector3 dir = (_heroMachine.Target.transform.position - _heroMachine.Owner.transform.position).normalized;
            Vector3 targetPos = _heroMachine.Target.transform.position;
            ToMove(targetPos);
            RotateToMoveDir(targetPos);
        }

        private void MoveToInputDir()
        {
            Vector2 inputDir = _heroMachine.MoveInput.normalized;
            Vector3 moveDir = new Vector3(inputDir.x, 0, inputDir.y);

            Vector3 destPos = _owner.transform.position + moveDir;
            if (Managers.MapManager.CanGo(destPos.z, destPos.x) == false)
                return;
            ToMove(destPos);
            RotateToMoveDir(destPos);
        }

        private void ToMove(Vector3 destPos)
        {
            _owner.transform.position = Vector3.MoveTowards(_owner.transform.position, destPos, _heroMachine.GetModifiedSpeed() * Time.deltaTime);
        }
        private void RotateToMoveDir(Vector3 target)
        {
            Vector3 targetDir = (target - _owner.transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDir);
            _owner.transform.rotation = Quaternion.Slerp(_owner.transform.rotation, targetRotation, 10 * Time.deltaTime);
        }

        private bool CheckChangeState()
        {
            if (_heroMachine.TargetMode == true)
            {
                return MoveToTargetOrUseSkill(); 
            }
            else if (_heroMachine.MoveInput == Vector2.zero)
            {
                _heroMachine.ChangeState(_heroMachine.IdleState);
                return true;
            }

            return false;
        }

        IEnumerator SendMyPos()
        {
            while (true)
            {
                _heroMachine.MovePacket.PosInfo.PosX = _owner.transform.position.x;
                _heroMachine.MovePacket.PosInfo.PosY = _owner.transform.position.y;
                _heroMachine.MovePacket.PosInfo.PosZ = _owner.transform.position.z;
                _heroMachine.MovePacket.PosInfo.RotY = _owner.transform.eulerAngles.y;
                _heroMachine.MovePacket.PosInfo.Speed = _heroMachine.MoveInput.magnitude * _heroMachine.GetModifiedSpeed();
                Managers.NetworkManager.Send(_heroMachine.MovePacket);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
