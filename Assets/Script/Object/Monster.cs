using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Creature
{
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Update()
    {
        base.Update();

    }

    public void Init(CreatureInfo info)
    {
        SetPos(gameObject, info.ObjectInfo.PosInfo);
        SetObjInfo(info);
    }
}
