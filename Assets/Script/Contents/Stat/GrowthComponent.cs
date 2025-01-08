using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthComponent
{
    public int Level
    {
        get { return Owner.MyHeroInfo.HeroInfo.LobbyHeroInfo.Level; }
        set { Owner.MyHeroInfo.HeroInfo.LobbyHeroInfo.Level = value; }
    }
    public long Exp
    {
        get { return Owner.MyHeroInfo.Exp; }
        set { Owner.MyHeroInfo.Exp = value; }
    }
    public long MaxExp { get; private set; }

    public MyHero Owner { get; private set; }
    public GrowthComponent(MyHero owner)
    {
        Owner = owner;
        SetMaxExp();
    }

    public void AddExp(int exp)
    {
        if (IsMaxLevel())
            return;
        Exp += exp;
        int preveLevel = Level;
        bool isUp = CheckLevelUp();
        if (isUp == true)
        {
            SetMaxExp();
            Owner.SkillComponent.LevelUp(preveLevel, Level);
        }
        UpdateGrowth();
    }

    public void UpdateGrowth()
    {
        Managers.EventBus.InvokeEvent(Enums.EventType.ChangeGrowth);
    }

    private bool CheckLevelUp()
    {
        bool isUp = false;

        while (true)
        {
            if (IsMaxLevel())
                break;

            if (Exp < GetExpAmountForNextLevel(Level))
                break;

            Exp = Math.Max(0, Exp - GetExpAmountForNextLevel(Level));
            Level++;
            isUp = true;
        }

        return isUp;
    }

    public long GetExpAmountForNextLevel(int level)
    {
        HeroStatData statData;
        if (Managers.DataManager.HeroStatDict.TryGetValue(level, out statData) == false)
            return 0;

        return statData.Exp;
    }

    public bool IsMaxLevel()
    {
        return Level == Managers.DataManager.HeroStatDict.Count;
    }

    private void SetMaxExp()
    {
        if (Managers.DataManager.HeroStatDict.TryGetValue(Level, out HeroStatData heroStatData) == true)
            MaxExp = heroStatData.Exp;
    }
}
