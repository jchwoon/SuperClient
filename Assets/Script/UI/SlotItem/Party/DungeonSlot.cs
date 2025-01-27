using Data;
using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DungeonSlot : BaseUI
{

    protected enum Texts
    {
        DungeonNameTxt,
        DungeonLevelTxt
    }

    [SerializeField]
    public Color SelectedSlotColor;
    [SerializeField]
    public Color NormalSlotColor;

    public RoomData DungeonRoomData { get; private set; }
    Action<int> _updateSelectedAction;
    Toggle _toggle;

    protected override void Awake()
    {
        base.Awake();

        Bind<TMP_Text>(typeof(Texts));

        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(Refresh);
    }

    public void SetInfo(RoomData dungeonRoomData, Action<int> updateSelectedAction)
    {
        DungeonRoomData = dungeonRoomData;
        _updateSelectedAction = updateSelectedAction;

        Get<TMP_Text>((int)Texts.DungeonNameTxt).text = dungeonRoomData.Name;
        Get<TMP_Text>((int)Texts.DungeonLevelTxt).text 
            = $"( Lv. {dungeonRoomData.MinRequiredLevel} ~ {dungeonRoomData.MaxRequiredLevel} )";

        if (_toggle.isOn)
        {
            SendReqAllPartyInfos();
        }
    }

    private void Refresh(bool isOn)
    {
        Image image = Utils.GetOrAddComponent<Image>(gameObject);
        if (isOn)
        {
            image.color = SelectedSlotColor;
            SendReqAllPartyInfos();
        }
        else
        {
            image.color = NormalSlotColor;
        }
    }

    private void SendReqAllPartyInfos()
    {
        if (DungeonRoomData == null)
            return;

        if (_updateSelectedAction != null)
        {
            _updateSelectedAction.Invoke(DungeonRoomData.RoomId);
        }

        Managers.PartyManager.SendReqAllPartyInfos(DungeonRoomData.RoomId);
    }
}
