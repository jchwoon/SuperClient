using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractUI : PopupUI
{
    enum GameObjects
    {
        InterLayout
    }
    static readonly int MAX_SLOT_COUNT = 5;
    GameObject _interactLayout;
    List<GameObject> _slots = new List<GameObject>(MAX_SLOT_COUNT);
    int _currentIdx = 0;

    protected override void Awake()
    {
        base.Awake();
        Bind<GameObject>(typeof(GameObjects));

        _interactLayout = Get<GameObject>((int)GameObjects.InterLayout);
        PreCreateSlot();
    }

    public GameObject AddSlot(string text)
    {
        if (_currentIdx >= MAX_SLOT_COUNT)
            return null;
        GameObject slot = _slots[_currentIdx];
        slot.SetActive(true);
        slot.GetComponent<InteractSlot>().SetSlot(text);
        _currentIdx++;
        return slot;
    }

    public void DeleteSlot(GameObject slot)
    {
        if (slot == null)
            return;
        slot.SetActive(false);
        _currentIdx--;
    }

    private void PreCreateSlot()
    {
        for (int i = 0; i < MAX_SLOT_COUNT; i++)
        {
            GameObject go = Managers.ResourceManager.Instantiate("InteractSlot", _interactLayout.transform);
            go.name = $"Slot{i + 1}";
            go.SetActive(false);
            _slots.Add(go);

        }
    }
    private void OnInterBtnClicked(PointerEventData eventData)
    {
        Debug.Log("gd");
    }
}
