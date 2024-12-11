using Data;
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
        DungeonSlot,
        PartySlot,
        PartyContents
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
    GameObject _dungeonSlot;
    GameObject _partySlot;

    GameObject _normalTab;
    GameObject _bossTab;

    GameObject _currentDungeonTab;
    GameObject _currentDungeonList;

    GameObject _partyContents;

    Color _activeColor = Color.gray;
    Color _deActiveColor = Color.white;

    //TODO 던전, 파티 클릭시 정보 받아오기
    [HideInInspector] public int dungeonId;
    [HideInInspector] public int partyId;
    [HideInInspector] public DungeonData dungeonData;

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
        _dungeonSlot = Get<GameObject>((int)GameObjects.DungeonSlot);
        _partySlot = Get<GameObject>((int)GameObjects.PartySlot);

        _partyContents = Get<GameObject>((int)GameObjects.PartyContents);

        BindEvent(Get<Button>((int)Buttons.SoloPartyBtn).gameObject, OnSinglePartyBtnClicked);
        BindEvent(Get<Button>((int)Buttons.PartyBtn).gameObject, OnPartyBtnClicked);
        BindEvent(Get<Button>((int)Buttons.PartyMakeBtn).gameObject, OnPartyMakeBtnClicked);
        BindEvent(Get<Button>((int)Buttons.PartyJoinBtn).gameObject, OnPartyJoinBtnClicked);
        BindEvent(Get<GameObject>((int)GameObjects.CloseBtn), OnCloseBtnClicked);
        BindEvent(Get<GameObject>((int)GameObjects.DungeonSlot), OnClickDungeonSlot);
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
        _partyBtns.SetActive(false);
        _partyListTab.SetActive(false);

        if (_currentDungeonList != null)
            _currentDungeonList.SetActive(false);

        changedDungeonList.SetActive(true);

        //TODO 현재 던전탭에 있는 파티들 동기화
        //GetDungeonInfo(changedDungeonList);

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

    private void OnClickDungeonSlot(PointerEventData eventData)
    {
        PartyTabOn(false);
    }

    private void OnPartyMakeBtnClicked(PointerEventData eventData)
    {
        Hero hero = Managers.ObjectManager.MyHero;
        
        Managers.PartyManager.MakeParty(hero);

        Managers.ResourceManager.Instantiate("PartyList", _partyContents.transform);
    }

    private void OnPartyJoinBtnClicked(PointerEventData eventData)
    {
        Hero hero = Managers.ObjectManager.MyHero;

        Managers.PartyManager.JoinParty(hero, partyId);
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
