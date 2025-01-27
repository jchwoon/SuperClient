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
    public List<MemberInfo> Members { get; private set; } = new List<MemberInfo>();

    public Party(MyHero hero, int roomId, long partyId)
    {
        DungeonRoomId = roomId;
        PartyId = partyId;
        MemberInfo info = new MemberInfo()
        {
            IsLeader = true,
            Class = hero.HeroInfo.LobbyHeroInfo.ClassType,
            Level = hero.GrowthInfo.Level,
            Name = hero.HeroInfo.LobbyHeroInfo.Nickname,
        };
        Members.Add(info);
    }

    public void AddMember()
    {

    }

    public void RemoveMember()
    {

    }

    public bool CheckLeader()
    {
        MemberInfo info = Members.Where(m => m.Name == Managers.ObjectManager.MyHero.Name).FirstOrDefault();

        return info.IsLeader;
    }
}
