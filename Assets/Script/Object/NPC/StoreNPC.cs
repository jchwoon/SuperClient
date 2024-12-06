using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreNPC : NPC
{
    public override void Interact()
    {
        StoreUI storeUI = Managers.UIManager.ShowPopup<StoreUI>();
        storeUI.Refresh();
    }
}
