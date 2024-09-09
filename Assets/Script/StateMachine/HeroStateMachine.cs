using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroStateMachine : StateMachine
{
    public Hero Hero { get; private set; }
    public HeroStateMachine(Hero hero)
    {
        Hero = hero;

    }


}
