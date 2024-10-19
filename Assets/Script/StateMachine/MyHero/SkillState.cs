using Data;
using Google.Protobuf.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.GridLayoutGroup;

namespace MyHeroState {
    public class SkillState : BaseState
    {
        public SkillState(MyHeroStateMachine heroMachine) : base(heroMachine)
        {

        }

        public override void Exit()
        {
            base.Exit();
            _heroMachine.SetAnimParameter(_heroMachine.Owner, _heroMachine.Owner.AnimData.SkillHash, false);
            if (_heroMachine.CurrentActiveSkillHash.HasValue)
                _heroMachine.SetAnimParameter(_heroMachine.Owner, _heroMachine.CurrentActiveSkillHash.Value, false);
        }
        public override void Enter()
        {
            base.Enter();
            _heroMachine.SetAnimParameter(_heroMachine.Owner, _heroMachine.Owner.AnimData.SkillHash, true);
        }
        public override void Update()
        {
            base.Update();
            if (_heroMachine.Owner.SkillComponent.isUsingSkill == true)
                return;

            if (_heroMachine.MoveInput != Vector2.zero)
            {
                _heroMachine.ChangeState(_heroMachine.MoveState);
                return;
            }

            if (_heroMachine.AttackMode == false || _heroMachine.Target == null)
            {
                _heroMachine.ChangeState(_heroMachine.IdleState);
                return;
            }

            MyHero owner = _heroMachine.Owner;
            BaseSkill skill = owner.SkillComponent.GetCanUseSkillAtReservedSkills(_heroMachine.Target);
            if (skill != null)
            {
                if (_heroMachine.isWaitSkillRes == true)
                    return;
                CoroutineHelper.Instance.StartHelperCoroutine(CoWailSkill());
                owner.SendUseSkill(skill.SkillId, _heroMachine.Target.ObjectId);
                return;
            }

            if (MoveToTargetOrUseSkill() == true)
                return;
        }

        //서버 부하를 줄이기 위해
        IEnumerator CoWailSkill()
        {
            _heroMachine.isWaitSkillRes = true;
            float time = 0.1f;
            float process = 0.0f;
            while (process < time)
            {
                process += Time.deltaTime;
                yield return null;
            }
            _heroMachine.isWaitSkillRes = false;
        }
    }
}
