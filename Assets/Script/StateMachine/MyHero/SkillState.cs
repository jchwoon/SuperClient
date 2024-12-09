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
        }
        public override void Enter()
        {
            base.Enter();
            _heroMachine.CreatureState = ECreatureState.Skill;
        }
        public override void Update()
        {
            base.Update();
        }

        ////서버 부하를 줄이기 위해
        //IEnumerator CoWaitSkill()
        //{
        //    _heroMachine.isWaitSkillRes = true;
        //    float time = 0.1f;
        //    float process = 0.0f;
        //    while (process < time)
        //    {
        //        process += Time.deltaTime;
        //        yield return null;
        //    }
        //    _heroMachine.isWaitSkillRes = false;
        //}
    }
}
