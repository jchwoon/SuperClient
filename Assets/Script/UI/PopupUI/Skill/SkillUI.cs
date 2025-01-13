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


        BindEvent(Get<GameObject>((int)GameObjects.CloseBtn), OnCloseBtnClicked);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Managers.EventBus.AddEvent(Enums.EventType.UpdateSkill, Refresh);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Managers.EventBus.RemoveEvent(Enums.EventType.UpdateSkill, Refresh);
    }

    public void Refresh()
    {
        _skillListUI.Refresh();
        _skillRegisterUI.SetInfo(OnSkillSlotClicked);
    }

    private void OnCloseBtnClicked(PointerEventData eventData)
    {
        Managers.SoundManager.PlayClick();
        ClosePopup<SkillUI>();
    }

    private void OnSkillSlotClicked(SkillData skillData)
    {
        Managers.SoundManager.PlayClick();
        _skillDescUI.Refresh(skillData);
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
