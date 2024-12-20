using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillUI : PopupUI
{
    enum GameObjects
    {
        CloseBtn,
        SkillListPanel,
        SkillDescPanel,
        SkillRegisterPanel
    }

    SkillListUI _skillListUI;
    SkillDescUI _skillDescUI;

    protected override void Awake()
    {
        base.Awake();

        Bind<GameObject>(typeof(GameObjects));

        _skillListUI = Get<GameObject>((int)GameObjects.SkillListPanel).GetComponent<SkillListUI>();
        _skillDescUI = Get<GameObject>((int)GameObjects.SkillDescPanel).GetComponent<SkillDescUI>();

        _skillListUI.RegisterAction(OnSkillSlotClicked);

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
}
