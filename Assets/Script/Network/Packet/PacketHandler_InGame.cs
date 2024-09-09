using Google.Protobuf.Protocol;
using Google.Protobuf;
using ServerCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PacketHandler
{
    public static void SpawnToCHandler(PacketSession session, IMessage packet)
    {
        SpawnToC spawnPacket = (SpawnToC)packet;
    }


}
