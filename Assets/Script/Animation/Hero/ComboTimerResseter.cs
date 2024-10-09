using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboTimerResseter : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MyHero hero = Managers.ObjectManager.MyHero;
        MyHeroStateMachine machine = hero.MyHeroStateMachine;
        machine.SetAnimParameter(hero, hero.AnimData.AttackComboHash, false);
    }
}   
