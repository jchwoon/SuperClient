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

        foreach (HeroInfo hero in spawnPacket.Heroes)
        {
            Managers.ObjectManager.Spawn(hero);
        }
    }

    public static void MoveToCHandler(PacketSession session, IMessage packet)
    {
        MoveToC movePacket = (MoveToC)packet;

        Hero hero = Managers.ObjectManager.Find<Hero>(movePacket.ObjectId);

        if (hero == null)
            return;

        hero.HeroMachine.UpdatePosInput(movePacket.PosInfo);
    }

    public static void DeSpawnToCHandler(PacketSession session, IMessage packet)
    {
        DeSpawnToC deSpawnPacket = (DeSpawnToC)packet;
        Debug.Log(deSpawnPacket.ObjectType);
        Managers.ObjectManager.DeSpawn(deSpawnPacket.ObjectId, deSpawnPacket.ObjectType);
    }
}
