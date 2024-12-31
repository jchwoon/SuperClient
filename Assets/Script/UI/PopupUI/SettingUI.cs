using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingUI : PopupUI
{
    enum GameObjects
    {
        CloseBtn,
        GameTab,
        SoundTab,
        AccountTab,
        TabMenuBar
    }
    enum Toggles
    {
        GameToggle,
        SoundToggle,
        AccountToggle
    }

    ToggleGroup _toggleGroup;

    Toggle _gameToggle;
    Toggle _soundToggle;
    Toggle _accountToggle;

    GameObject _gameTab;
    GameObject _soundTab;
    GameObject _accountTab;

    protected override void Awake()
    {
        base.Awake();

        Bind<GameObject>(typeof(GameObjects));
        Bind<Toggle>(typeof(Toggles));

        _gameTab = Get<GameObject>((int)GameObjects.GameTab);
        _soundTab = Get<GameObject>((int)GameObjects.SoundTab);
        _accountTab = Get<GameObject>((int)GameObjects.AccountTab);

        _toggleGroup = Get<GameObject>((int)GameObjects.TabMenuBar).GetComponent<ToggleGroup>();

        _gameToggle = Get<Toggle>((int)Toggles.GameToggle);
        _soundToggle = Get<Toggle>((int)Toggles.SoundToggle);
        _accountToggle = Get<Toggle>((int)Toggles.AccountToggle);

        BindEvent(_gameToggle.gameObject, OnToggleClicked);
        BindEvent(_soundToggle.gameObject, OnToggleClicked);
        BindEvent(_accountToggle.gameObject, OnToggleClicked);
        BindEvent(Get<GameObject>((int)GameObjects.CloseBtn), (_) => ClosePopup<SettingUI>());
    }

    public void Refresh()
    {
        Clear();
        ChangeTab();
    }

    private void ChangeTab()
    {
        if (_gameToggle.isOn)
        {
            _gameTab.SetActive(true);
            return;
        }

        if (_soundToggle.isOn)
        {
            _soundTab.SetActive(true);
            return;
        }

        if (_accountToggle)
        {
            _accountTab.SetActive(true);
            return;
        }
    }

    private void OnToggleClicked(PointerEventData eventData)
    {
        Refresh();
    }

    private void Clear()
    {
        _gameTab.SetActive(false);
        _soundTab.SetActive(false);
        _accountTab.SetActive(false);
    }


}
