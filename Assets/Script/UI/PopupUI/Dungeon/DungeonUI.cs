using Data;
using Google.Protobuf.Enum;
using Google.Protobuf.Struct;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Enums;

public class DungeonUI : PopupUI
{
    enum GameObjects
    {
        CloseBtn,
        CreatePartyBtn,
        NormalDungeonSlotContent,
        BossDungeonSlotContent,
        PartySlotContent,
        DungeonTab,
        PartyTab,
        NormalDungeonView,
        BossDungeonView
    }

    enum Toggles
    {
        DungeonToggle,
        PartyToggle,
        NormalToggle,
        BossToggle
    }

    [SerializeField]
    public Color SelectedToggleColor;
    [SerializeField]
    public Color NormalToggleColor;

    GameObject _dungeonTab;
    GameObject _partyTab;

    GameObject _normalDungeonView;
    GameObject _bossDungeonView;

    GameObject _normalDungeonSlotContent;
    GameObject _bossDungeonSlotContent;
    GameObject _partySlotContent;

    //MainTab
    Toggle _dungeonToggle;
    Toggle _partyToggle;
    Toggle _prevMainToggle;

    //DungeonTypeTab
    Toggle _normalToggle;
    Toggle _bossToggle;
    Toggle _prevDungeonTypeToggle;

    List<RoomData> _normalDungeonDatas;
    List<RoomData> _bossDungeonDatas;

    int _dungeonCount;
    int? _selectedDungeonRoomId = null;

    protected override void Awake()
    {
        base.Awake();

        Bind<GameObject>(typeof(GameObjects));
        Bind<Toggle>(typeof(Toggles));

        _dungeonTab = Get<GameObject>((int)GameObjects.DungeonTab);
        _partyTab = Get<GameObject>((int)GameObjects.PartyTab);

        _normalDungeonView = Get<GameObject>((int)GameObjects.NormalDungeonView);
        _bossDungeonView = Get<GameObject>((int)GameObjects.BossDungeonView);

        _normalDungeonSlotContent = Get<GameObject>((int)GameObjects.NormalDungeonSlotContent);
        _bossDungeonSlotContent = Get<GameObject>((int)GameObjects.BossDungeonSlotContent);
        _partySlotContent = Get<GameObject>((int)GameObjects.PartySlotContent);

        _dungeonToggle = Get<Toggle>((int)Toggles.DungeonToggle);
        _partyToggle = Get<Toggle>((int)Toggles.PartyToggle);
        _normalToggle = Get<Toggle>((int)Toggles.NormalToggle);
        _bossToggle = Get<Toggle>((int)Toggles.BossToggle);

        _normalDungeonDatas = Managers.DataManager.RoomDict.Values.Where(r => r.DungeonType == EDungeonType.Normal).ToList();
        _bossDungeonDatas = Managers.DataManager.RoomDict.Values.Where(r => r.DungeonType == EDungeonType.Boss).ToList();

        for (int i = 0; i < _normalDungeonDatas.Count; i++)
        {
            GameObject go = _normalDungeonSlotContent.transform.GetChild(i).gameObject;
            Utils.GetOrAddComponent<Toggle>(go).group = _normalDungeonSlotContent.GetComponent<ToggleGroup>();
        }


        for (int i = 0; i < _bossDungeonDatas.Count; i++)
        {
            GameObject go = _bossDungeonSlotContent.transform.GetChild(i).gameObject;
            Utils.GetOrAddComponent<Toggle>(go).group = _bossDungeonSlotContent.GetComponent<ToggleGroup>();
        }


        BindEvent(_dungeonToggle.gameObject, OnChangedMainTab);
        BindEvent(_partyToggle.gameObject, OnChangedMainTab);
        BindEvent(_normalToggle.gameObject, OnChangedDungeonTypeTab);
        BindEvent(_bossToggle.gameObject, OnChangedDungeonTypeTab);
        BindEvent(Get<GameObject>((int)GameObjects.CreatePartyBtn), OnCreatePartyBtnClicked);
        BindEvent(Get<GameObject>((int)GameObjects.CloseBtn), OnCloseBtnClicked);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Managers.EventBus.AddEvent(Enums.EventType.UpdatePartyInfos, RefreshPartyListByDungeon);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Managers.EventBus.RemoveEvent(Enums.EventType.UpdatePartyInfos, RefreshPartyListByDungeon);
    }

    public void Refresh()
    {
        ClearMainTabs();
        RefreshByMainTab();
    }

    private void RefreshByMainTab()
    {
        if (_dungeonToggle.isOn)
        {
            _dungeonTab.SetActive(true);
            RefreshByDungeonType();
            UpdateToggleColors(_prevMainToggle, _dungeonToggle);
            _prevMainToggle = _dungeonToggle;
            return;
        }

        if (_partyToggle.isOn)
        {
            _partyTab.SetActive(true);
            _partyTab.GetComponent<PartyTabUI>().Refresh();
            UpdateToggleColors(_prevMainToggle, _partyToggle);
            _prevMainToggle = _partyToggle;
            return;
        }
    }

    private void RefreshByDungeonType()
    {
        ClearDungeonList();
        ClearPartyList();

        if (_normalToggle.isOn)
        {
            _normalDungeonView.SetActive(true);
            for (int i = 0; i < _normalDungeonDatas.Count; i++)
            {
                GameObject go = _normalDungeonSlotContent.transform.GetChild(i).gameObject;
                go.SetActive(true);
                DungeonSlot slot = Utils.GetOrAddComponent<DungeonSlot>(go);
                slot.SetInfo(_normalDungeonDatas[i], UpdateSelectedDungeonRoomId);
            }
            _dungeonCount = _normalDungeonDatas.Count;
            UpdateToggleColors(_prevDungeonTypeToggle, _normalToggle);
            _prevDungeonTypeToggle = _normalToggle;
            return;
        }

        if (_bossToggle.isOn)
        {
            _bossDungeonView.SetActive(true);
            for (int i = 0; i < _bossDungeonDatas.Count; i++)
            {
                GameObject go = _bossDungeonSlotContent.transform.GetChild(i).gameObject;
                go.SetActive(true);
                DungeonSlot slot = Utils.GetOrAddComponent<DungeonSlot>(go);
                slot.SetInfo(_bossDungeonDatas[i], UpdateSelectedDungeonRoomId);
            }
            _dungeonCount = _bossDungeonDatas.Count;
            UpdateToggleColors(_prevDungeonTypeToggle, _bossToggle);
            _prevDungeonTypeToggle = _bossToggle;
            return;
        }
    }
    //선택한 던전에 따라 파티 리스트를 리프레쉬
    private void RefreshPartyListByDungeon()
    {
        ClearPartyList();
        List<PartyInfo> partyInfos = Managers.PartyManager.PartyInfos;
        for (int i = 0; i < partyInfos.Count; i++)
        {
            PartySlot slot = Managers.UIManager.GenerateSlot<PartySlot>(_partySlotContent.transform);
            slot.SetInfo(partyInfos[i]);
        }
    }

    private void OnCreatePartyBtnClicked(PointerEventData eventData)
    {
        EFailReasonCreateParty failReason = Managers.PartyManager.CheckAndSendReqCreateParty(_selectedDungeonRoomId);
        switch (failReason)
        {
            case EFailReasonCreateParty.Exist:
                Managers.UIManager.ShowAlertPopup("이미 파티가 존재합니다.", AlertBtnNum.One);
                break;
            case EFailReasonCreateParty.NotSelected:
                Managers.UIManager.ShowAlertPopup("선택된 던전이 없습니다.", AlertBtnNum.One);
                break;
        }
    }

    private void OnCloseBtnClicked(PointerEventData eventData)
    {
        ClosePopup<DungeonUI>();
    }

    private void OnChangedMainTab(PointerEventData eventData)
    {
        if (eventData.pointerClick.gameObject == _prevMainToggle.gameObject)
            return;

        Refresh();
    }

    private void OnChangedDungeonTypeTab(PointerEventData eventData)
    {
        if (eventData.pointerClick.gameObject == _prevDungeonTypeToggle.gameObject)
            return;

        RefreshByDungeonType();
    }

    private void ClearMainTabs()
    {
        _dungeonTab.SetActive(false);
        _partyTab.SetActive(false);
    }

    private void ClearDungeonList()
    {
        _normalDungeonView.SetActive(false);
        _bossDungeonView.SetActive(false);
    }

    private void ClearPartyList()
    {
        for (int i = 0; i < _partySlotContent.transform.childCount; i++)
        {
            GameObject go = _partySlotContent.transform.GetChild(i).gameObject;
            Managers.ResourceManager.Destroy(go, isPool: true);
        }
    }

    private void UpdateSelectedDungeonRoomId(int roomId)
    {
        _selectedDungeonRoomId = roomId;
    }

    private void UpdateToggleColors(Toggle prevToggle, Toggle currentToggle)
    {
        if (prevToggle != null)
        {
            prevToggle.gameObject.GetComponent<Image>().color = NormalToggleColor;
        }
        currentToggle.gameObject.GetComponent<Image>().color = SelectedToggleColor;
    }
}
