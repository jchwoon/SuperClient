using Data;
using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DungeonSlot : BaseUI
{

    protected enum Texts
    {
        DungeonNameTxt,
        DungeonLevelTxt
    }

    public RoomData DungeonRoomData { get; private set; }
    Action<int> _slotClickAction;

    protected override void Awake()
    {
        base.Awake();

        Bind<TMP_Text>(typeof(Texts));

        BindEvent(gameObject, OnSlotClicked);
    }

    public void SetInfo(RoomData dungeonRoomData, Action<int> slotClickAction)
    {
        DungeonRoomData = dungeonRoomData;
        _slotClickAction = slotClickAction;

        Get<TMP_Text>((int)Texts.DungeonNameTxt).text = dungeonRoomData.Name;
        Get<TMP_Text>((int)Texts.DungeonLevelTxt).text 
            = $"( Lv. {dungeonRoomData.MinRequiredLevel} ~ {dungeonRoomData.MaxRequiredLevel} )";
    }

    private void OnSlotClicked(PointerEventData eventData)
    {
        if (DungeonRoomData == null)
            return;
        
        if (_slotClickAction != null)
        {
            _slotClickAction.Invoke(DungeonRoomData.RoomId);
        }
        Managers.PartyManager.SendReqAllPartyInfos(DungeonRoomData.RoomId);
    }
}
