using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.GridLayoutGroup;

namespace MyHeroState
{
    public class MoveState : BaseState
    {
        float _currentTime;
        float _sendMovePacketCycle = 0.3f;

        public MoveState(MyHeroStateMachine heroMachine) : base(heroMachine)
        {
        }

        public override void Exit()
        {
            base.Exit();
            MyHero owner = _heroMachine.Owner;
            _heroMachine.SetAnimParameter(owner, owner.AnimData.MoveSpeedHash, 0.0f);
        }
        public override void Enter()
        {
            base.Enter();
            MyHero owner = _heroMachine.Owner;
            _heroMachine.SetAnimParameter(owner, owner.AnimData.MoveSpeedHash, _heroMachine.Owner.Stat.StatInfo.MoveSpeed);
            _heroMachine.CreatureState = ECreatureState.Move;
        }

        public override ECreatureState GetCreatureState()
        {
            return ECreatureState.Move;
        }

        public override void Update()
        {
            base.Update();

            CheckAndMove();
        }

        private void CheckAndMove()
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
            _owner.transform.position = Vector3.MoveTowards(_owner.transform.position, destPos, _heroMachine.Owner.Stat.StatInfo.MoveSpeed * Time.deltaTime);
            _currentTime += Time.deltaTime;
            if (_currentTime >= _sendMovePacketCycle)
                SendMyPos();
        }
        private void RotateToMoveDir(Vector3 target)
        {
            Vector3 targetDir = (target - _owner.transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDir);
            _owner.transform.rotation = Quaternion.Slerp(_owner.transform.rotation, targetRotation, 10 * Time.deltaTime);
        }

        private void SendMyPos()
        {
            if (_owner == null)
                return;

            _heroMachine.MovePacket.PosInfo.PosX = _owner.transform.position.x;
            _heroMachine.MovePacket.PosInfo.PosY = _owner.transform.position.y;
            _heroMachine.MovePacket.PosInfo.PosZ = _owner.transform.position.z;
            _heroMachine.MovePacket.PosInfo.RotY = _owner.transform.eulerAngles.y;
            Managers.NetworkManager.Send(_heroMachine.MovePacket);
            _currentTime = 0;
        }
    }
}
