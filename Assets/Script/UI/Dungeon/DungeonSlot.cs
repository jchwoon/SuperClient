using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DungeonSlot : BaseUI
{

    protected enum Texts
    {
        DungeonNameTxt,
        DungeonLvTxt
    }



    protected override void Awake()
    {
        base.Awake();

        Bind<TMP_Text>(typeof(Texts));
    }
}
