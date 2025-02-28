using Data;
using Google.Protobuf.Enum;
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

    //파티 가입 요청
    public void ReqApplyParty(long partyId)
    {
        ReqApplyPartyToS applyPartyPacket = new ReqApplyPartyToS();
        applyPartyPacket.PartyId = partyId;

        Managers.NetworkManager.Send(applyPartyPacket);
    }

    //파티 지원자들의 정보가 업데이트 됐을때 핸들링
    public void HandleUpdateApplierInfos(List<ApplierInfo> applierInfos)
    {
        if (applierInfos == null)
            return;

        MyParty.UpdateApplier(applierInfos);
    }

    public void HandleUpdateMemberInfos(List<MemberInfo> memberInfos)
    {
        if (memberInfos == null)
            return;

        MyParty.UpdateMember(memberInfos);
    }

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

    #region 파티 생성
    //파티 생성 요청
    public EFailReasonCreateParty CheckAndSendReqCreateParty(int? roomId)
    {
        EFailReasonCreateParty failReason = CheckCreateParty(roomId);

        if (failReason == EFailReasonCreateParty.None)
        {
            SendReqCreateParty(roomId.Value);
        }

        return failReason;
    }

    private EFailReasonCreateParty CheckCreateParty(int? roomId)
    {
        if (MyParty != null)
            return EFailReasonCreateParty.Exist;
        if (roomId == null)
            return EFailReasonCreateParty.NotSelected;


        return EFailReasonCreateParty.None;
    }

    private void SendReqCreateParty(int roomId)
    {
        ReqCreatePartyToS createPartyPacket = new ReqCreatePartyToS();
        createPartyPacket.RoomId = roomId;

        Managers.NetworkManager.Send(createPartyPacket);
    }

    public void HandleResCreateParty(int roomId, long partyId)
    {
        //파티 생성시엔 leader는 자기 자신
        MyHero hero = Managers.ObjectManager.MyHero;
        MyParty = new Party(hero, roomId, partyId);
        SendReqAllPartyInfos(roomId);
    }
    #endregion

    #region 파티 탈퇴
    public void LeaveParty()
    {
        if (MyParty == null)
            return;
        ReqLeavePartyToS leavePartyPacket = new ReqLeavePartyToS();
        Managers.NetworkManager.Send(leavePartyPacket);
    }
    #endregion

    public void Clear()
    {
        MyParty = null;
    }
}
