using Data;
using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager
{
    public DungeonData DungeonData { get; private set; }
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

    public void MakeParty(Hero owner)
    {
        CreatePartyToS createPartyPacket = new CreatePartyToS();
        //createPartyPacket.DungeonId = roomId;
        createPartyPacket.Hero = owner.HeroInfo;

        Managers.NetworkManager.Send(createPartyPacket);
    }

    public void JoinParty()
    {

    }
}
