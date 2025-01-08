using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : BaseUI
{
    enum Images
    {
        SkillImage
    }
    enum GameObjects
    {
        SkillIcon,
        LevelUpBtn
    }
    enum Texts
    {
        SkillNameTxt,
        SkillLvTxt
    }

    Action<SkillData> _slotClickAction;
    Action _skillRegisterAction;
    Action<Vector2> _slotDragEvent;
    Action<Sprite> _slotBeginDragEvent;

    Image _skillImage;
    Button _levelUpBtn;

    public SkillData SkillData { get; private set; }

    protected override void Awake()
    {
        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));
        Bind<TMP_Text>(typeof(Texts));

        GameObject skillIcon = Get<GameObject>((int)GameObjects.SkillIcon);
        GameObject levelUpBtn = Get<GameObject>((int)GameObjects.LevelUpBtn);
        _levelUpBtn = levelUpBtn.GetComponent<Button>();

        BindEvent(levelUpBtn, OnLevelUpBtnClicked);
        BindEvent(gameObject, OnSlotClicked);
        BindEvent(gameObject, OnSlotBeginDrag, Enums.TouchEvent.BeginDrag);
        BindEvent(gameObject, OnSkillDrag, Enums.TouchEvent.Drag);
        BindEvent(gameObject, OnSkillPointerUp, Enums.TouchEvent.EndDrag);
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

    public void SetInfo(SkillData skillData, Action<SkillData> slotClickAction, Action<Sprite> slotBeginDragEvent = null, Action<Vector2> slotDragEvent = null, Action skillRegisterAction = null)
    {
        _slotClickAction = slotClickAction;
        _slotBeginDragEvent = slotBeginDragEvent;
        _slotDragEvent = slotDragEvent;
        _skillRegisterAction = skillRegisterAction;
        SkillData = skillData;

        Get<TMP_Text>((int)Texts.SkillNameTxt).text = skillData.SkillName;

        _skillImage = Get<Image>((int)Images.SkillImage);
        _skillImage.sprite = Managers.ResourceManager.GetResource<Sprite>(skillData.IconName);
        _skillImage.gameObject.SetActive(true);

        Refresh();
    }

    public void Refresh()
    {
        SkillComponent skillComponent = Utils.GetMySkillComponent();
        if (skillComponent != null)
        {
            BaseSkill skill = skillComponent.GetSkillById(SkillData.TemplateId);
            Get<TMP_Text>((int)Texts.SkillLvTxt).text = skill.CurrentSkillLevel.ToString();
            if (SkillData.MaxLevel <= skill.CurrentSkillLevel)
            {
                _levelUpBtn.interactable = false;
            }
            else
            {
                _levelUpBtn.interactable = true;
            }
        }
    }

    private void OnLevelUpBtnClicked(PointerEventData eventData)
    {
        Managers.SoundManager.PlayClick();
        SkillComponent skillComponent = Utils.GetMySkillComponent();
        skillComponent.CheckAndSendLevelUpPacket(SkillData.TemplateId);
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
