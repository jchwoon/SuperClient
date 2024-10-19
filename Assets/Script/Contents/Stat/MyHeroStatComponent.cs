using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyHeroStatComponent : StatComponent
{
    public MyHeroStatComponent(Creature owner) : base(owner)
    {
    }

    public override void InitStat(StatInfo statInfo)
    {
        base.InitStat(statInfo);
    }
    public override void UpdateStat()
    {
        base.UpdateStat();
        Managers.EventBus.InvokeEvent(Enums.EventType.ChangeStat);
    }
}
