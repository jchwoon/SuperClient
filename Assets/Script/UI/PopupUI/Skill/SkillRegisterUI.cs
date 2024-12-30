using Data;
using Google.Protobuf.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillRegisterUI : BaseUI
{
    enum GameObjects
    {
        AllClearBtn,
        OneClearBtn,
    }

    SkillRegisterSlot _selectedSlot;
    public SkillRegisterSlot SkillRegisterSlot
    {
        get { return _selectedSlot; }
        set
        {
            HighlightSelectedSlot(value);
            _selectedSlot = value; 
        }
    }

    SkillRegisterSlot[] _slots = new SkillRegisterSlot[3];

    Action OnChangedSelectedSlot;
    Action<SkillData> _slotClickEvent;

    protected override void Awake()
    {
        Bind<GameObject>(typeof(GameObjects));

        BindEvent(Get<GameObject>((int)GameObjects.AllClearBtn), OnAllClearBtnClicked);
        BindEvent(Get<GameObject>((int)GameObjects.OneClearBtn), OnOneClearBtnClicked);
    }

    public void SetInfo(Action<SkillData> slotClickEvent)
    {
        _slotClickEvent = slotClickEvent;
        SkillRegisterSlot[] slots = gameObject.GetComponentsInChildren<SkillRegisterSlot>();
        slots.CopyTo(_slots, 0);
        for (int i = 0; i < slots.Length; i++)
        {
            SkillComponent skill = Utils.GetMySkillComponent();
            slots[i].SetInfo(GameSettings.GetSkillSlotTemplateId(i));
            slots[i].RegisterEvent(OnSlotClicked, CheckDuplicateSkill, OnSlotChanged);
        }
    }

    private bool CheckDuplicateSkill(ESkillSlotType slotType)
    {
        foreach (SkillRegisterSlot slot in _slots)
        {
            SkillData skillData = slot.SkillData;
            if (skillData != null && skillData.SkillSlotType == slotType)
            {
                 return true;
            }
        }

        return false;
    }

    private void OnSlotClicked(SkillData skillData, SkillRegisterSlot selectSlot)
    {
        SkillRegisterSlot = selectSlot;
        if (_slotClickEvent != null)
        {
            _slotClickEvent.Invoke(skillData);
        }
    }

    private void OnSlotChanged()
    {
        for (int i = 0; i <  _slots.Length; i++)
        {
            GameSettings.SetSkillSlot(i, _slots[i].SkillData == null ? 0 : _slots[i].SkillData.TemplateId);
        }

        Managers.EventBus.InvokeEvent<SkillRegisterSlot[]>(Enums.EventType.UpdateSkillSet, _slots);
    }

    private void OnAllClearBtnClicked(PointerEventData eventData)
    {
        foreach (SkillRegisterSlot slot in _slots)
        {
            slot.Clear();
        }
        SkillRegisterSlot = null;
        OnSlotChanged();
    }

    private void OnOneClearBtnClicked(PointerEventData eventData)
    {
        if (_selectedSlot == null)
            return;

        SkillRegisterSlot.Clear();
        SkillRegisterSlot = null;
        OnSlotChanged();
    }

    private void HighlightSelectedSlot(SkillRegisterSlot selectedSlot)
    {
        foreach (var slot in _slots)
        {
            slot.GetComponent<Image>().color = Color.white;
        }
        if (selectedSlot != null)
        {
            selectedSlot.GetComponent<Image>().color = Color.red;
        }
    }
}
