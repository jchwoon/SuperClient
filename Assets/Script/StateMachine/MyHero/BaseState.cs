using Google.Protobuf.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.GridLayoutGroup;

namespace MyHeroState
{
    public class BaseState : IState
    {
        protected MyHeroStateMachine _heroMachine;
        protected Creature _owner;

        public BaseState(MyHeroStateMachine heroMachine)
        {
            _heroMachine = heroMachine;
            _owner = heroMachine.Owner;
        }
        public virtual void Exit()
        {

        }
        public virtual void Enter()
        {
        }

        public virtual void Update()
        {

        }
        public virtual ECreatureState GetCreatureState()
        {
            return ECreatureState.Idle;
        }

        //Attacking이 true일 때 호출될 함수
        public bool MoveToTargetOrUseSkill()
        {
            if (_heroMachine.Attacking == false || _heroMachine.Target == null)
            {
                if (_heroMachine.CurrentState == _heroMachine.IdleState)
                    return false;
                _heroMachine.ChangeState(_heroMachine.IdleState);
                return true;
            }

            float dist = Vector3.Distance(_heroMachine.Owner.transform.position, _heroMachine.Target.transform.position);
            BaseSkill skill = _heroMachine.Owner.SkillComponent.GetCanUseSkillAtReservedSkills(_heroMachine.Target);


            if (dist > (skill == null ? 1.0f : _heroMachine.Owner.SkillComponent.GetSkillRange(skill)))
            {
                if (_heroMachine.CurrentState == _heroMachine.MoveState)
                    return false;
                _heroMachine.ChangeState(_heroMachine.MoveState);
                return true;
            }


            if (_heroMachine.CurrentState == _heroMachine.SkillState)
                return false;
            _heroMachine.ChangeState(_heroMachine.SkillState);
            return true;

            //return false;
        }
    }
}

