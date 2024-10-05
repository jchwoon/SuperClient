using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf.Enum;
using Data;

public class ComboExitHandler : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MyHeroStateMachine machine = Managers.ObjectManager.MyHero.MyHeroStateMachine;
        machine.SkillState.StopComboExitRoutine();
        machine.Attacking = false;
        machine.ChangeState(machine.IdleState);

    }
}
