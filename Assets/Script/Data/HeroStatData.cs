using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroStatData
{
    public float MoveSpeed { get; private set; }
    public float AtkSpeed { get; private set; }

    public void SetStat(StatInfo statInfo)
    {
        MoveSpeed = statInfo.MoveSpeed;
        AtkSpeed = statInfo.AtkSpeed;
    }
}
