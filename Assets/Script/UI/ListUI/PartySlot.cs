using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PartySlot : BaseUI
{

    protected enum Texts
    {
        Member1Name,
        Member2Name,
        Member3Name,
        Member4Name,
        Member1LV,
        Member2LV,
        Member3LV,
        Member4LV,
    }


    protected override void Awake()
    {
        base.Awake();

        Bind<TMP_Text>(typeof(Texts));
    }


}
