using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

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
        }

        public override void Update()
        {
            base.Update();

            if (_heroMachine.MoveInput == Vector2.zero && _heroMachine.TargetMode == false)
            {
                //attacking이면
                //if (_heroMachine.Attacking)
                //{
                //    //스킬 거리 내에 타겟이 있다면 
                //    float dist = _heroMachine.GetDistToTarget();
                //    //현재 선택된 스킬의 정보
                //    _heroMachine.ChangeState(_heroMachine.SkillState);
                //}
                _heroMachine.ChangeState(_heroMachine.IdleState);
                return;
            }

            _heroMachine.SetAnimParameter(_owner, _owner.AnimData.MoveSpeedHash, GetModifiedSpeed());
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
            Vector3 targetPos = _heroMachine.Target.transform.position;
            _owner.transform.position = Vector3.MoveTowards(_owner.transform.position, targetPos, GetModifiedSpeed() * Time.deltaTime);
            RotateToMoveDir(targetPos);
        }

        private void MoveToInputDir()
        {
            Vector2 inputDir = _heroMachine.MoveInput.normalized;
            Vector3 moveDir = new Vector3(inputDir.x, 0, inputDir.y);

            Vector3 pos = _owner.transform.position + moveDir;
            if (Managers.MapManager.CanGo(pos.z, pos.x) == false)
                return;
            _owner.transform.position = Vector3.MoveTowards(_owner.transform.position, pos, GetModifiedSpeed() * Time.deltaTime);
            RotateToMoveDir(pos);
        }

        private void RotateToMoveDir(Vector3 target)
        {
            Vector3 targetDir = (target - _owner.transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDir);
            _owner.transform.rotation = Quaternion.Slerp(_owner.transform.rotation, targetRotation, 10 * Time.deltaTime);
        }

        private float GetModifiedSpeed()
        {
            return 20 * _heroMachine.MoveRatio;
        }

        private void CheckChangeState()
        {
            if (_heroMachine.MoveInput == Vector2.zero && _heroMachine.Attacking == false)
            {
                _heroMachine.ChangeState(_heroMachine.IdleState);
                return;
            }
        }

        IEnumerator SendMyPos()
        {
            while (true)
            {
                _heroMachine.MovePacket.PosInfo.PosX = _owner.transform.position.x;
                _heroMachine.MovePacket.PosInfo.PosY = _owner.transform.position.y;
                _heroMachine.MovePacket.PosInfo.PosZ = _owner.transform.position.z;
                _heroMachine.MovePacket.PosInfo.RotY = _owner.transform.eulerAngles.y;
                _heroMachine.MovePacket.PosInfo.Speed = _heroMachine.MoveInput.magnitude * GetModifiedSpeed();
                Managers.NetworkManager.Send(_heroMachine.MovePacket);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
