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

    Action<GameObject> _refreshSlotAction;
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
    public void SetSlotInfo(LobbyHeroInfo heroInfo, Action<GameObject> action)
    {
        _refreshSlotAction = action;

        _levelTxt.text = heroInfo.Level.ToString();
        _nickname.text = heroInfo.Nickname;
        SetClassIcon(heroInfo.ClassType);
    }

    private void OnSlotClicked(PointerEventData eventData)
    {
        Debug.Log("Clicked");
        _refreshSlotAction?.Invoke(gameObject);
    }

    private void SetClassIcon(EHeroClassType classType)
    {
        switch (classType)
        {
            case EHeroClassType.Warrior:
                _classIcon.sprite = Managers.ResourceManager.GetResource<Sprite>("Warrior");
                return;
            case EHeroClassType.Archer:
                _classIcon.sprite = Managers.ResourceManager.GetResource<Sprite>("Archer");
                return;
            case EHeroClassType.Mage:
                _classIcon.sprite = Managers.ResourceManager.GetResource<Sprite>("Mage");
                return;
            default:
                return;
        }
    }
}
