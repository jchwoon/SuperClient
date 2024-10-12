using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyComponent
{
    public int Gold { get; private set; }


    public void InitCurrency(int gold)
    {
        Gold = gold;
        UpdateCurrency();
    }

    public void UpdateCurrency()
    {
        Managers.EventBus.InvokeEvent(Enums.EventType.ChangeCurrency);
    }
}
