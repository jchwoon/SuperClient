using Data;
using Google.Protobuf.Enum;
using Google.Protobuf.Struct;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestMyHe : Hero
{
    public TestMachine MyHeroStateMachine { get; private set; }

    protected override void Start()
    {
        Init();
    }

    protected override void Update()
    {
        if (MyHeroStateMachine == null)
            return;
        MyHeroStateMachine.Update();
    }

    public void Init()
    {
        MyHeroStateMachine = new TestMachine(this);


    }
}
