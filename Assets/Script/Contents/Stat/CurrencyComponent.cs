using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyComponent
{
    public int Gold
    {
        get { return Owner.MyHeroInfo.Gold; }
        set
        {
            Owner.MyHeroInfo.Gold = value;
            UpdateCurrency();
        }
    }
    public MyHero Owner { get; private set; }

    public CurrencyComponent(MyHero owner)
    {
        Owner = owner;
    }

    public void InitCurrency(int gold)
    {
        Gold = gold;
    }
    public void AddGold(int gold)
    {
        Gold += gold;
    }    
    public void RemoveGold(int gold)
    {
        Gold -= gold;
    }

    private void UpdateCurrency()
    {
        Managers.EventBus.InvokeEvent(Enums.EventType.ChangeCurrency);
    }
}
