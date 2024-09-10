using Google.Protobuf.Protocol;
using Google.Protobuf;
using ServerCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf.Struct;

public partial class PacketHandler
{
    public static void SpawnToCHandler(PacketSession session, IMessage packet)
    {
        SpawnToC spawnPacket = (SpawnToC)packet;
        Debug.Log("hi");

        foreach (HeroInfo hero in spawnPacket.Heroes)
        {
            Debug.Log(hero);
            Managers.ObjectManager.Spawn(hero);
        }
    }


}
