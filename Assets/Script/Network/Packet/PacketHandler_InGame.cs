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
        foreach (CreatureInfo creature in spawnPacket.Creatures)
        {
            Managers.ObjectManager.Spawn(creature);
        }
    }

    public static void MoveToCHandler(PacketSession session, IMessage packet)
    {
        MoveToC movePacket = (MoveToC)packet;

        GameObject go = Managers.ObjectManager.FindById(movePacket.ObjectId);

        if (go == null)
            return;
        BaseObject bo = go.GetComponent<BaseObject>();

        if (bo != null && bo.Machine != null)
        {
            bo.Machine.UpdatePosInput(movePacket.PosInfo);
        }

    }

    public static void DeSpawnToCHandler(PacketSession session, IMessage packet)
    {
        DeSpawnToC deSpawnPacket = (DeSpawnToC)packet;
        Managers.ObjectManager.DeSpawn(deSpawnPacket.ObjectId, deSpawnPacket.ObjectType);
    }
}
