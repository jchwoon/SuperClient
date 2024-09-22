using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour
{
    private StateMachine _stateMachine;
    public int ObjectId { get; set; }
    public StateMachine Machine
    {
        get { return _stateMachine; }
        protected set { _stateMachine = value; }
    }
    protected virtual void Awake()
    {
        //�ڽ� Ŭ�������� Machine ����
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
