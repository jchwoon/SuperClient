using Google.Protobuf.Enum;
using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StatComponent
{
    MyHero _owner;
    public StatComponent(MyHero owner)
    {
        _owner = owner;
    }

    public void UpdateStat()
    {
        Managers.EventBus.InvokeEvent(Enums.EventType.ChangeStat);
    }

    public void HandleModifyStat(StatInfo statInfo)
    {
        if (_owner.StatInfo == null)
            return;

        _owner.StatInfo.MergeFrom(statInfo);
        UpdateStat();
    }
}
