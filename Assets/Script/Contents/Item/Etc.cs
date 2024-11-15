using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Etc : Item
{
    public EtcData EtcData { get; private set; }
    public Etc(int itemId) : base(itemId)
    {
        EtcData = (EtcData)ItemData;
    }
}
