using Google.Protobuf.Enum;
using Google.Protobuf.Struct;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSlot : BaseUI
{
    enum Images
    {
        ClassIcon
    }
    enum Texts
    {
        Nickname,
        Level
    }

    Action<GameObject, int> _refreshSlotAction;
    int _slotIdx;
    TMP_Text _levelTxt;
    TMP_Text _nickname;
    Image _classIcon;
    protected override void Awake()
    {
        base.Awake();

        Bind<TMP_Text>(typeof(Texts));
        Bind<Image>(typeof(Images));

        _levelTxt = Get<TMP_Text>((int)Texts.Level);
        _nickname = Get<TMP_Text>((int)Texts.Nickname);
        _classIcon = Get<Image>((int)Images.ClassIcon);

        BindEvent(gameObject, OnSlotClicked);
    }
    public void SetSlotInfo(LobbyHeroInfo heroInfo, Action<GameObject, int> action, int idx)
    {
        _refreshSlotAction = action;
        _slotIdx = idx;

        _levelTxt.text = $"{heroInfo.Level} LV";
        _nickname.text = heroInfo.Nickname;
        SetClassIcon(heroInfo.ClassType);
    }

    private void OnSlotClicked(PointerEventData eventData)
    {
        _refreshSlotAction?.Invoke(gameObject, _slotIdx);
    }

    private void SetClassIcon(EHeroClassType classType)
    {
        switch (classType)
        {
            case EHeroClassType.Guardian:
                _classIcon.sprite = Managers.ResourceManager.GetResource<Sprite>("Warrior_sprite");
                return;
            case EHeroClassType.Archer:
                _classIcon.sprite = Managers.ResourceManager.GetResource<Sprite>("Archer_sprite");
                return;
            case EHeroClassType.Mage:
                _classIcon.sprite = Managers.ResourceManager.GetResource<Sprite>("Mage_sprite");
                return;
            default:
                return;
        }
    }
}
