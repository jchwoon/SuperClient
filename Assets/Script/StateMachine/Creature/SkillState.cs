using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            _machine.SetAnimParameter(_owner, _owner.AnimData.SkillHash, false);
        }
        public override void Enter()
        {
            base.Enter();
            _machine.SetAnimParameter(_owner, _owner.AnimData.SkillHash, true);
        }
    }
}