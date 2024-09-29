using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf.Enum;
using Data;

public class AttackText : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MyHeroStateMachine machine = Managers.ObjectManager.MyHero.MyHeroStateMachine;
        //if (machine.GetAnimParameter(_)
        machine.AttackState.StopComboExitRoutine();
        Debug.Log("Combo 5 Exit");
        machine.Attacking = false;
        machine.ChangeState(machine.IdleState);
    }
}
