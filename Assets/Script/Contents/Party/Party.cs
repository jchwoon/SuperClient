using Data;
using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Party 
{
    public int DungeonRoomId { get; private set; }
    public long PartyId { get; private set; }
    public List<MemberInfo> Members { get; private set; } = new List<MemberInfo>(MAX_PARTY_MEMBER_COUNT);
    public List<ApplierInfo> Appliers { get; private set; } = new List<ApplierInfo>();

    const int MAX_PARTY_MEMBER_COUNT = 4;

    public Party(MyHero hero, int roomId, long partyId)
    {
        DungeonRoomId = roomId;
        PartyId = partyId;
        MemberInfo info = new MemberInfo()
        {
            IsLeader = true,
            Info = new LobbyHeroInfo()
            {
                ClassType = hero.HeroInfo.LobbyHeroInfo.ClassType,
                Level = hero.GrowthInfo.Level,
                Nickname = hero.HeroInfo.LobbyHeroInfo.Nickname,
            },
            ObjectId = hero.ObjectId
        };
        Members.Add(info);
    }

    public void AddMember()
    {
    }

    public void RemoveMember()
    {

    }

    public void UpdateMember(List<MemberInfo> memberInfos)
    {
        Members = memberInfos;
        Managers.EventBus.InvokeEvent(Enums.EventType.UpdatePartyMembers);
    }

    public void UpdateApplier(List<ApplierInfo> applierInfos)
    {
        Appliers = applierInfos;
        Managers.EventBus.InvokeEvent(Enums.EventType.UpdatePartyApplier);
    }

    public bool IsPartyLeader()
    {
        MemberInfo info = Members.Where(m => m.ObjectId == Managers.ObjectManager.MyHero.ObjectId).FirstOrDefault();

        return info.IsLeader;
    }
}
