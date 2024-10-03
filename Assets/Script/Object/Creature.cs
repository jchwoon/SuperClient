using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Creature : BaseObject
{
    public Animator Animator { get; private set; }
    public AnimationData AnimData { get; private set; }
    public NavMeshAgent Agent { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Animator = transform.GetComponent<Animator>();
        AnimData = new AnimationData();
        Agent = GetComponent<NavMeshAgent>();
        if (isMachineInit == false)
        {
            Machine = new CreatureMachine(this);
            isMachineInit = true;
        }
    }
    protected override void Update()
    {
        base.Update();
    }

    public virtual void SetInfo(CreatureInfo info)
    {
        ObjectId = info.ObjectInfo.ObjectId;
    }
}
