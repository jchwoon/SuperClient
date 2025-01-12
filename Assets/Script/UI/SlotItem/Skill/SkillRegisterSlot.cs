using Data;
using Google.Protobuf.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillRegisterSlot : BaseUI
{
    enum Images
    {
        SkillImage
    }

    Image _skillImage;
    Action<SkillData, SkillRegisterSlot> _slotClickEvent;
    Action _slotChangeEvent;
    Func<ESkillSlotType, bool> _checkDuplicateEvent;
    public ActiveSkillData SkillData { get; private set; }
    protected override void Awake()
    {
        Bind<Image>(typeof(Images));

        _skillImage = Get<Image>((int)Images.SkillImage);

        BindEvent(gameObject, OnSlotClicked);
        BindEvent(gameObject, OnDropSkill, Enums.TouchEvent.Drop);
    }

    public void RegisterEvent(Action<SkillData, SkillRegisterSlot> slotClickEvent, Func<ESkillSlotType, bool> checkDuplicateEvent, Action slotChangeEvent)
    {
        _slotClickEvent = slotClickEvent;
        _checkDuplicateEvent = checkDuplicateEvent;
        _slotChangeEvent = slotChangeEvent;
    }

    public void SetInfo(int templateId)
    {
        Managers.DataManager.ActiveSkillDict.TryGetValue(templateId, out ActiveSkillData skillData);
        SkillData = skillData;
        if (skillData != null)
        {
            _skillImage.gameObject.SetActive(true);
            _skillImage.sprite = Managers.ResourceManager.GetResource<Sprite>(SkillData.IconName);
        }
    }

    private void OnSlotClicked(PointerEventData eventData)
    {
        if (_slotClickEvent == null || SkillData == null)
            return;

        _slotClickEvent.Invoke(SkillData, this);
    }

    private void OnDropSkill(PointerEventData eventData)
    {
        SkillData = CheckValidAndGetSkillData(eventData);

        if (SkillData == null) return;

        Managers.SoundManager.PlayDropped();
        _skillImage.gameObject.SetActive(true);
        _skillImage.sprite = Managers.ResourceManager.GetResource<Sprite>(SkillData.IconName);
        if (_slotChangeEvent != null)
        {
            _slotChangeEvent.Invoke();
        }
    }

    private ActiveSkillData CheckValidAndGetSkillData(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return null;
        SkillSlot slot = eventData.pointerDrag.GetComponent<SkillSlot>();

        if (slot == null || slot.SkillData == null)
            return null;

        if (_checkDuplicateEvent.Invoke(slot.SkillData.SkillSlotType))
            return null;

        ActiveSkillData activeSkillData = slot.SkillData as ActiveSkillData;
        if (activeSkillData == null)
            return null;

        return activeSkillData;
    }

    public void Clear()
    {
        SkillData = null;
        _skillImage.sprite = null;
        _skillImage.gameObject.SetActive(false);
    }
}
