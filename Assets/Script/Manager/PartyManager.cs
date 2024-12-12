using Data;
using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private HashSet<int> usedPartyIds = new HashSet<int>(); // 사용 중인 파티 ID를 추적
    private int nextPartyId = 0; // 새로운 파티 ID를 생성할 때 사용할 기준값


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

    public void MakeParty(Hero owner,GameObject partyTab, DungeonData dungeonData = null)
    {
        int newPartyId = GenerateNewPartyId();
        Party newParty = new Party { PartyId = newPartyId };
        newParty.PartyTab = partyTab;

        if (partyTab.TryGetComponent(out PartySlot partySlot))
        {
            partySlot.PartyId = newPartyId;
        }

        parties.Add(newParty);

        AddPartyMember(owner, newPartyId);

        CreatePartyToS createPartyPacket = new CreatePartyToS();

        createPartyPacket.Hero = owner.HeroInfo;

        Managers.NetworkManager.Send(createPartyPacket);

        OnPartyUpdated?.Invoke();
    }

    public void JoinParty(Hero hero, int partyId)
    {
        AddPartyMember(hero, partyId);
        OnPartyUpdated?.Invoke();
    }

    public void AddPartyMember(Hero owner, int partyId)
    {
        Party party = GetParty(partyId);
        if (party == null)
            return; // 파티가 존재하지 않으면 종료

        string name = owner.Name;
        int level = owner.Level;

        if (party.PartyMembers.Count >= 4)
            return;

        if (party.PartyMembers.Exists(m => m.Name == name))
            return;

        party.PartyMembers.Add(new PartyMember(name, level));

        owner.PartyId = partyId;
    }


    public void RemovePartyMember(Hero hero, int partyId)
    {
        Party party = GetParty(partyId);
        if (party == null)
            return; // 파티가 존재하지 않으면 종료

        string name = hero.Name;

        var member = party.PartyMembers.Find(m => m.Name == name);
        if (!string.IsNullOrEmpty(member.Name))
        {
            party.PartyMembers.Remove(member);

            if (party.PartyMembers.Count == 0)
            {
                GameObject partyTabObject = party.PartyTab.gameObject;                
                UnityEngine.Object.Destroy(partyTabObject);
                
                parties.Remove(party);
                usedPartyIds.Remove(partyId); // 사용된 ID 반환
            }

            OnPartyUpdated?.Invoke();
        }
    }

    private int GenerateNewPartyId()
    {
        while (usedPartyIds.Contains(nextPartyId))
        {
            nextPartyId++;
        }
        usedPartyIds.Add(nextPartyId);
        return nextPartyId;
    }

    private bool IsPartyIdValid(int partyId)
    {
        return parties.Any(p => p.PartyId == partyId);
    }

    public Party GetParty(int partyId)
    {
        return parties.FirstOrDefault(p => p.PartyId == partyId);
    }

}
