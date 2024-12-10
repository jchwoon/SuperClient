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
        PartyListTab,
        DungeonSlot
    }

    enum Buttons
    {
        SoloPartyBtn,
        PartyBtn,
        PartyMakeBtn,
        PartyJoinBtn,
    }

    GameObject _normalDungeonList;
    GameObject _bossDungeonList;

    GameObject _partyBtns;
    GameObject _partyListTab;

    GameObject _normalTab;
    GameObject _bossTab;

    GameObject _currentDungeonTab;
    GameObject _currentDungeonList;


    Color _activeColor = Color.gray;
    Color _deActiveColor = Color.white;

    protected override void Awake()
    {
        base.Awake();

        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));

        _normalTab = Get<GameObject>((int)GameObjects.NormalDungeonBtn);
        _normalDungeonList = Get<GameObject>((int)GameObjects.NormalDungeonList);
        _bossTab = Get<GameObject>((int)GameObjects.BossDungeonBtn);
        _bossDungeonList = Get<GameObject>((int)GameObjects.BossDungeonList);

        _partyBtns = Get<GameObject>((int)GameObjects.PartyBtns);
        _partyListTab = Get<GameObject>((int)GameObjects.PartyListTab);
        
        BindEvent(Get<Button>((int)Buttons.SoloPartyBtn).gameObject, OnSinglePartyBtnClicked);
        BindEvent(Get<Button>((int)Buttons.PartyBtn).gameObject, OnPartyBtnClicked);
        BindEvent(Get<Button>((int)Buttons.PartyMakeBtn).gameObject, OnPartyMakeBtnClicked);
        BindEvent(Get<Button>((int)Buttons.PartyJoinBtn).gameObject, OnPartyJoinBtnClicked);
        BindEvent(Get<GameObject>((int)GameObjects.DungeonSlot), OnPartyMakeBtnClicked);
        BindEvent(Get<GameObject>((int)GameObjects.CloseBtn), OnCloseBtnClicked);
        BindEvent(_normalTab, (eventData) => { OnChangeTab(eventData, _normalTab, _normalDungeonList); });
        BindEvent(_bossTab, (eventData) => { OnChangeTab(eventData, _bossTab, _bossDungeonList); });

    }

    protected override void Start()
    {
        base.Start();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        Initial();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    private void Initial()
    {
        _normalDungeonList.SetActive(false);
        _bossDungeonList.SetActive(false);
        _partyBtns.SetActive(false);
        _partyListTab.SetActive(false);
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
        PartyTabOn(false);

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
    
    private void OnPartyBtnClicked(PointerEventData eventData)
    {
        PartyTabOn(true);
    }

    private void OnPartyMakeBtnClicked(PointerEventData eventData)
    {
        string heroName = Managers.ObjectManager.MyHero.Name;
        Managers.PartyManager.MakeParty(Managers.ObjectManager.MyHero);
    }

    private void OnPartyJoinBtnClicked(PointerEventData eventData)
    {

    }
    private void PartyTabOn(bool isPartyTabOpen)
    {
        _partyBtns.SetActive(!isPartyTabOpen);
        _partyListTab.SetActive(isPartyTabOpen);
    }
    

    private void GetDungeonInfo(GameObject changedDungeonList)
    {
        if (changedDungeonList.TryGetComponent(out DungeonSlot dungeonSlot))
        {

        }
    }

}
