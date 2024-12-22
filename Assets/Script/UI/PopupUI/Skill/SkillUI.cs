using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillUI : PopupUI
{
    enum GameObjects
    {
        CloseBtn,
        SkillListPanel,
        SkillDescPanel,
        SkillRegisterPanel,
        DraggingSkill
    }

    SkillListUI _skillListUI;
    SkillDescUI _skillDescUI;
    SkillRegisterUI _skillRegisterUI;
    DraggingSkillSlot _draggingSlot;

    protected override void Awake()
    {
        base.Awake();

        Bind<GameObject>(typeof(GameObjects));

        _skillListUI = Get<GameObject>((int)GameObjects.SkillListPanel).GetComponent<SkillListUI>();
        _skillDescUI = Get<GameObject>((int)GameObjects.SkillDescPanel).GetComponent<SkillDescUI>();
        _skillRegisterUI = Get<GameObject>((int)GameObjects.SkillRegisterPanel).GetComponent<SkillRegisterUI>();
        _draggingSlot = Get<GameObject>((int)GameObjects.DraggingSkill).GetComponent<DraggingSkillSlot>(); ;

        _skillListUI.RegisterEvent(OnSkillSlotClicked, OnSkillSlotBeginDrag, OnSkillSlotDrag, OnSkillRegisterEvent);
        _skillRegisterUI.SetInfo(OnSkillSlotClicked);

        BindEvent(Get<GameObject>((int)GameObjects.CloseBtn), OnCloseBtnClicked);
    }

    public void Refresh()
    {
        _skillListUI.Refresh();
    }

    private void OnCloseBtnClicked(PointerEventData eventData)
    {
        ClosePopup<SkillUI>();
    }

    private void OnSkillSlotClicked(SkillData skillData)
    {
        _skillDescUI.OnSlotClicked(skillData);
    }

    private void OnSkillSlotBeginDrag(Sprite sprite)
    {
        _draggingSlot.OnBeginDrag(sprite);
    }
    private void OnSkillSlotDrag(Vector2 screenPos)
    {
        _draggingSlot.OnDrag(screenPos, CanvasRect);
    }

    private void OnSkillRegisterEvent()
    {
        _draggingSlot.OnEndDrag();
    }
}
