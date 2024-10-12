using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthComponent
{
    public int Level {  get; private set; }
    public int Exp { get; private set; }
    public int MaxExp { get; private set; }

    public void InitGrowth(int level, int exp)
    {
        Level = level;
        Exp = exp;

        if (Managers.DataManager.HeroStatDict.TryGetValue(level, out HeroStatData heroStatData) == true)
            MaxExp = heroStatData.Exp;

        UpdateGrowth();
    }

    public void UpdateGrowth()
    {
        Managers.EventBus.InvokeEvent(Enums.EventType.ChangeGrowth);
    }
}
