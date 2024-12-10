using Google.Protobuf.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DungeonUI : PopupUI
{
    enum GameObjects
    {
        CloseBtn,
        NormalDungeonBtn,
        NormalDungeonList,
        BossDungeonBtn,
        BossDungeonList,
        PartyBtns,
        SoloPartyBtn,
        PartyBtn,
        Party,
        DungeonPartyList
    }

    GameObject _normalDungeonList;
    GameObject _bossDungeonList;
    GameObject _partyList;

    GameObject _partyTab;
    GameObject _partyBtnTab;

    GameObject _normalBtn;
    GameObject _bossBtn;
    GameObject _soloPartyBtn;
    GameObject _PartyBtn;
    GameObject _partyMakeBtn;

    GameObject _currentDungeonTab;
    GameObject _currentDungeonList;


    Color _activeColor = Color.gray;
    Color _deActiveColor = Color.white;

    protected override void Awake()
    {
        base.Awake();

        Bind<GameObject>(typeof(GameObjects));

        _normalDungeonList = Get<GameObject>((int)GameObjects.NormalDungeonList);
        _bossDungeonList = Get<GameObject>((int)GameObjects.BossDungeonList);
        _partyList = Get<GameObject>((int)GameObjects.DungeonPartyList);
        _partyTab = Get<GameObject>((int)GameObjects.Party);
        _normalBtn = Get<GameObject>((int)GameObjects.NormalDungeonBtn);
        _bossBtn = Get<GameObject>((int)GameObjects.BossDungeonBtn);
        _soloPartyBtn = Get<GameObject>((int)GameObjects.SoloPartyBtn);
        _PartyBtn = Get<GameObject>((int)GameObjects.PartyBtn);

        BindEvent(Get<GameObject>((int)GameObjects.CloseBtn), OnCloseBtnClicked);
        BindEvent(_normalBtn, (eventData) => { OnChangeTab(eventData, _normalBtn, _normalDungeonList); });
        BindEvent(_bossBtn, (eventData) => { OnChangeTab(eventData, _bossBtn, _bossDungeonList); });
        BindEvent(_soloPartyBtn, OnSinglePartyBtnClicked);
        BindEvent(_partyMakeBtn, OnMakePartyBtnClicked);
    }

    protected override void Start()
    {
        base.Start();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
    private void OnCloseBtnClicked(PointerEventData eventData)
    {
        ClosePopup<DungeonUI>();
    }
    private void OnChangeTab(PointerEventData eventData, GameObject changedTab, GameObject changedDungeonList)
    {
        if (changedTab == _currentDungeonTab)
            return;

        ChangeTabColor(changedTab);
        ChangeDungeonList(changedDungeonList);
    }
    private void ChangeTabColor(GameObject changedTab)
    {
        if (_currentDungeonTab != null)
            _currentDungeonTab.GetComponent<Image>().color = _deActiveColor;

        changedTab.GetComponent<Image>().color = _activeColor;
        _currentDungeonTab = changedTab;
    }

    private void ChangeDungeonList(GameObject changedDungeonList)
    {
        PartyMakeTabOn(false);

        if (_currentDungeonList != null)
            _currentDungeonList.SetActive(false);

        changedDungeonList.SetActive(true);

        //TODO 현재 던전탭에 있는 파티들 동기화
        GetDungeonInfo(changedDungeonList);

        _currentDungeonList = changedDungeonList;
    }

    private void OnSinglePartyBtnClicked(PointerEventData eventData)
    {
        Managers.MapManager.ChangeMap(3);
    }

    private void OnMakePartyBtnClicked(PointerEventData eventData)
    {
        PartyMakeTabOn(true);
    }

    private void PartyMakeTabOn(bool isPartyTabOpen)
    {
        _partyTab.SetActive(!isPartyTabOpen);
        _partyList.SetActive(isPartyTabOpen);
    }

    private void GetDungeonInfo(GameObject changedDungeonList)
    {
        if (changedDungeonList.TryGetComponent(out DungeonSlot dungeonSlot))
        {

        }
    }
}
