using Google.Protobuf.Enum;
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

    public void AddGold(int gold)
    {
        Gold += gold;
    }    
    public void RemoveGold(int gold)
    {
        Gold -= gold;
    }

    public bool CheckEnoughCurrency(ECurrencyType currencyType, int value)
    {
        //Temp 나중에 Currency가 추가 되면 수정
        return Gold >= value;
    }

    private void UpdateCurrency()
    {
        Managers.EventBus.InvokeEvent(Enums.EventType.ChangeCurrency);
    }
}
