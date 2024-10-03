using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour
{
    public int ObjectId { get; set; }
    public virtual StateMachine Machine { get; set; }
    protected bool isMachineInit = false;

    protected virtual void Awake()
    {
        if (isMachineInit == false)
            Machine = new StateMachine();
    }
    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        if (Machine == null)
            return;
        Machine.Update();
    }
}
