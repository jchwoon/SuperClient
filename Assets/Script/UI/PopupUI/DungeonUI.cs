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
        DungeonSlotContent,
        PartySlotContent,
        DungeonTab,
        PartyTab
    }

    enum Toggles
    {
        DungeonToggle,
        PartyToggle,
        NormalToggle,
        BossToggle
    }

    [SerializeField]
    public Color SelectedColor;
    [SerializeField]
    public Color NormalColor;

    GameObject _dungeonTab;
    GameObject _partyTab;

    GameObject _dungeonSlotContent;
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
    int _selectedDungeonRoomId;

    protected override void Awake()
    {
        base.Awake();

        Bind<GameObject>(typeof(GameObjects));
        Bind<Toggle>(typeof(Toggles));

        _dungeonTab = Get<GameObject>((int)GameObjects.DungeonTab);
        _partyTab = Get<GameObject>((int)GameObjects.PartyTab);

        _dungeonSlotContent = Get<GameObject>((int)GameObjects.DungeonSlotContent);
        _partySlotContent = Get<GameObject>((int)GameObjects.PartySlotContent);

        _dungeonToggle = Get<Toggle>((int)Toggles.DungeonToggle);
        _partyToggle = Get<Toggle>((int)Toggles.PartyToggle);
        _normalToggle = Get<Toggle>((int)Toggles.NormalToggle);
        _bossToggle = Get<Toggle>((int)Toggles.BossToggle);

        _normalDungeonDatas = Managers.DataManager.RoomDict.Values.Where(r => r.DungeonType == EDungeonType.Normal).ToList();
        _bossDungeonDatas = Managers.DataManager.RoomDict.Values.Where(r => r.DungeonType == EDungeonType.Boss).ToList();

        BindEvent(_dungeonToggle.gameObject, (_) => OnChangedMainTab());
        BindEvent(_partyToggle.gameObject, (_) => OnChangedMainTab());
        BindEvent(_normalToggle.gameObject, (_) => OnChangedDungeonTypeTab());
        BindEvent(_bossToggle.gameObject, (_) => OnChangedDungeonTypeTab());
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
            UpdateToggleColors(_prevMainToggle, _partyToggle);
            _prevMainToggle = _partyToggle;
            return;
        }
    }

    private void RefreshByDungeonType()
    {
        ClearDungeonList();

        if (_normalToggle.isOn)
        {
            for (int i = 0; i < _normalDungeonDatas.Count; i++)
            {
                GameObject go = _dungeonSlotContent.transform.GetChild(i).gameObject;
                go.SetActive(true);
                Utils.GetOrAddComponent<DungeonSlot>(go).SetInfo(_normalDungeonDatas[i], UpdateSelectedDungeonRoomId);
            }
            _dungeonCount = _normalDungeonDatas.Count;
            UpdateToggleColors(_prevDungeonTypeToggle, _normalToggle);
            _prevDungeonTypeToggle = _normalToggle;
            return;
        }

        if (_bossToggle.isOn)
        {
            for (int i = 0; i < _bossDungeonDatas.Count; i++)
            {
                GameObject go = _dungeonSlotContent.transform.GetChild(i).gameObject;
                go.SetActive(true);
                Utils.GetOrAddComponent<DungeonSlot>(go).SetInfo(_bossDungeonDatas[i], UpdateSelectedDungeonRoomId);
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
            case EFailReasonCreateParty.PartyAlreadyExist:
                Managers.UIManager.ShowAlertPopup("이미 파티가 존재합니다.", AlertBtnNum.One);
                break;
        }
    }

    private void OnCloseBtnClicked(PointerEventData eventData)
    {
        ClosePopup<DungeonUI>();
    }

    private void OnChangedMainTab()
    {
        Refresh();
    }

    private void OnChangedDungeonTypeTab()
    {
        RefreshByDungeonType();
    }

    private void ClearMainTabs()
    {
        _dungeonTab.SetActive(false);
        _partyTab.SetActive(false);
    }

    private void ClearDungeonList()
    {
        for (int i = 0; i < _dungeonCount; i++)
        {
            Transform child = _dungeonSlotContent.transform.GetChild(i);
            child.gameObject.SetActive(false);
        }
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
            prevToggle.gameObject.GetComponent<Image>().color = NormalColor;
        }
        currentToggle.gameObject.GetComponent<Image>().color = SelectedColor;
    }
}
