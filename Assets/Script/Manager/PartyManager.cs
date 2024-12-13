using Data;
using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private List<Party> parties = new List<Party>();
    public DungeonData DungeonData { get; private set; }

    public event Action OnPartyUpdated;

    public Transform Parent
    {
        get
        {
            GameObject party = GameObject.Find("Party");
            if (party == false)
                party = new GameObject("Party");
            return party.transform;
        }
    }

    public void MakeParty(Hero owner, DungeonData dungeonData = null)
    {
        int newPartyId = parties.Count;
        Party newParty = new Party { PartyId = newPartyId };

        parties.Add(newParty);

        AddPartyMember(owner.Name, owner.Level, newPartyId);

        CreatePartyToS createPartyPacket = new CreatePartyToS();


        Managers.NetworkManager.Send(createPartyPacket);

        OnPartyUpdated?.Invoke();
    }

    public void JoinParty(Hero hero, int partyId)
    {
        AddPartyMember(hero.Name, hero.Level, partyId);
        OnPartyUpdated?.Invoke();
    }

    public void AddPartyMember(string name, int level, int partyId)
    {
        Party party = parties[partyId];

        if (party.PartyMembers.Count >= 4)
            return;
        if (party.PartyMembers.Exists(m => m.Name == name))
            return;


        party.PartyMembers.Add(new PartyMember(name, level));
    }

    public void RemovePartyMember(string name, int partyId)
    {
        Party party = parties[partyId];
        var member = party.PartyMembers.Find(m => m.Name == name);
        if (!string.IsNullOrEmpty(member.Name))
        {
            party.PartyMembers.Remove(member);
            OnPartyUpdated?.Invoke();
        }
    }

    public Party GetParty(int partyId)
    {
        return parties[partyId];
    }
}
