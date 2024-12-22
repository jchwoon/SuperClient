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
    Action _skillRegisterAction;
    Action<Vector2> _slotDragEvent;
    Action<Sprite> _slotBeginDragEvent;
    Image _skillImage;

    public SkillData SkillData { get; private set; }

    protected override void Awake()
    {
        Bind<Image>(typeof(Images));

        BindEvent(gameObject, OnSlotClicked);
        BindEvent(gameObject, OnSlotBeginDrag, Enums.TouchEvent.BeginDrag);
        BindEvent(gameObject, OnSkillDrag, Enums.TouchEvent.Drag);
        BindEvent(gameObject, OnSkillPointerUp, Enums.TouchEvent.EndDrag);
    }

    public void SetInfo(SkillData skillData, Action<SkillData> slotClickAction, Action<Sprite> slotBeginDragEvent = null, Action<Vector2> slotDragEvent = null, Action skillRegisterAction = null)
    {
        _slotClickAction = slotClickAction;
        _slotBeginDragEvent = slotBeginDragEvent;
        _slotDragEvent = slotDragEvent;
        _skillRegisterAction = skillRegisterAction;
        SkillData = skillData;

        _skillImage = Get<Image>((int)Images.SkillImage);
        _skillImage.sprite = Managers.ResourceManager.GetResource<Sprite>(skillData.IconName);
        _skillImage.gameObject.SetActive(true);
    }

    private void OnSlotClicked(PointerEventData eventData)
    {
        _slotClickAction.Invoke(SkillData);
    }

    private void OnSlotBeginDrag(PointerEventData eventData)
    {
        if (_slotBeginDragEvent != null)
        {
            _slotBeginDragEvent.Invoke(_skillImage.sprite);
        }
    }

    private void OnSkillDrag(PointerEventData eventData)
    {
        if (_slotDragEvent != null)
        {
            _slotDragEvent.Invoke(eventData.position);
        }
    }

    private void OnSkillPointerUp(PointerEventData eventData)
    {
        if (_skillRegisterAction != null)
        {
            _skillRegisterAction.Invoke();
        }
    }
}
