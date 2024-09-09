using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HeroState
{
    public class WalkState : BaseState
    {
        public WalkState(HeroStateMachine heroMachine) : base(heroMachine)
        {
        }

        public override void Exit()
        {
            base.Exit();

            StopAnim(_heroMachine.Hero.AnimData.WalkHash);
        }
        public override void Enter()
        {
            base.Enter();

            StartAnim(_heroMachine.Hero.AnimData.WalkHash);
        }

        public override void Update()
        {
            base.Update();

            if (_heroMachine.MoveInput == Vector2.zero)
            {
                _heroMachine.ChangeState(_heroMachine.IdleState);
                return;
            }

            _heroMachine.Hero.transform.position += new Vector3(_heroMachine.MoveInput.x, 0, _heroMachine.MoveInput.y) * 2 * Time.deltaTime;
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(_heroMachine.MoveInput.x, 0, _heroMachine.MoveInput.y));
            _heroMachine.Hero.transform.rotation = Quaternion.Slerp(_heroMachine.Hero.transform.rotation, targetRotation, 7 * Time.deltaTime);
        }
    }

}
