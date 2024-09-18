using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseNPC : BaseObject
{
    private void OnTriggerEnter(Collider other)
    {
        Managers.UIManager.ShowPopup<InteractUI>();
    }
}
