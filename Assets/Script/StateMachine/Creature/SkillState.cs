using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf.Enum;

namespace CreatureState
{
    public class SkillState : BaseState
    {
        public SkillState(CreatureMachine creatureMachine) : base(creatureMachine)
        {
        }

        public override void Exit()
        {
            base.Exit();
        }
        public override void Enter()
        {
            base.Enter();
            _machine.CreatureState = ECreatureState.Skill;
        }
    }
}