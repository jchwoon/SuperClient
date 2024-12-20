using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : BaseUI
{
    enum Images
    {
        SkillImage
    }

    Action<SkillData> _slotClickAction;
    SkillData _skillData;

    protected override void Awake()
    {
        Bind<Image>(typeof(Images));

        BindEvent(gameObject, OnSlotClicked);
    }

    public void SetInfo(SkillData skillData, Action<SkillData> slotClickAction)
    {
        _slotClickAction = slotClickAction;
        _skillData = skillData;
        Image image = Get<Image>((int)Images.SkillImage);
        image.sprite = Managers.ResourceManager.GetResource<Sprite>(skillData.IconName);
        image.gameObject.SetActive(true);
    }

    private void OnSlotClicked(PointerEventData eventData)
    {
        _slotClickAction.Invoke(_skillData);
    }
}
