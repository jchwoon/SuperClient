using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : BaseObject
{
    public Animator Animator { get; private set; }
    public AnimationData AnimData { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        Animator = GetComponent<Animator>();
        AnimData = new AnimationData();
    }
}
