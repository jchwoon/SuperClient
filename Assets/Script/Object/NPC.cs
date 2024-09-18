using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : BaseObject
{
    protected InteractUI _interUI;
    GameObject _slot;
    private void OnTriggerEnter(Collider other)
    {
        _interUI = Managers.UIManager.ShowPopup<InteractUI>();
        _slot = _interUI.AddSlot($"��ȭ�ϱ�");
    }

    private void OnTriggerExit(Collider other)
    {
        if (_slot != null)
            _interUI.DeleteSlot(_slot);
    }
}
