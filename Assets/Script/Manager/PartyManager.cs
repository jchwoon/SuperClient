using Data;
using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Enums;

public struct PartyMember
{
    public string Name;
    public int Level;

    public PartyMember(string name, int level)
    {
        Name = name;
        Level = level;
    }
}

public class PartyManager
{
    public Party MyParty { get; private set; }
    public List<PartyInfo> PartyInfos = new List<PartyInfo>();

    //��Ƽ ���� ��û
    public void ReqJoinParty(long partyId)
    {
        ReqJoinPartyToS reqJoinPartyPacket = new ReqJoinPartyToS();
        reqJoinPartyPacket.PartyId = partyId;

        Managers.NetworkManager.Send(reqJoinPartyPacket);
    }
    //��Ƽ ���� ��û�� ���� �ڵ鸵
    public void HandleReqJoinApproval(Hero applier)
    {
        if (applier == null)
            return;

        //UI���˸� ����
        //Managers.UIManager.ShowPopup();
        //applyer.MapName
    }

    #region ��Ƽ ����
    //��Ƽ ���� ��û
    public EFailReasonCreateParty CheckAndSendReqCreateParty(int roomId)
    {
        EFailReasonCreateParty failReason = CheckCreatePart();

        if (failReason == EFailReasonCreateParty.None)
        {
            SendReqCreateParty(roomId);
        }

        return failReason;
    }

    private EFailReasonCreateParty CheckCreatePart()
    {
        if (MyParty != null)
            return EFailReasonCreateParty.PartyAlreadyExist;

        return EFailReasonCreateParty.None;
    }

    private void SendReqCreateParty(int roomId)
    {
        CreatePartyToS createPartyPacket = new CreatePartyToS();
        createPartyPacket.RoomId = roomId;

        Managers.NetworkManager.Send(createPartyPacket);
    }

    public void HandleResCreateParty()
    {
        MyParty = new Party();
    }
    #endregion

    public void SendReqAllPartyInfos(int roomId)
    {
        ReqAllPartyInfoToS reqAllPartyInfoPacket = new ReqAllPartyInfoToS();
        reqAllPartyInfoPacket.RoomId = roomId;
        Managers.NetworkManager.Send(reqAllPartyInfoPacket);
    }

    public void HandleResAllPartyInfos(List<PartyInfo> partyInfos)
    {
        PartyInfos = partyInfos;
        Managers.EventBus.InvokeEvent(Enums.EventType.UpdatePartyInfos);
    }
}
