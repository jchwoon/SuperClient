using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public partial class PacketHandler
{
    public static void ConnectToCHandler(PacketSession session ,IMessage packet)
    {
        ConnectToC connectPacket = (ConnectToC)packet;
    }

    public static void ResHeroListToCHandler(PacketSession session, IMessage packet)
    {
        ResHeroListToC heroListPacket = (ResHeroListToC)packet;

        LobbyScene lobbyScene = (LobbyScene)Managers.SceneManagerEx.CurrentScene;
        lobbyScene.OnReceiveServerData(heroListPacket);
    }
    public static void ResCreateHeroToCHandler(PacketSession session, IMessage packet)
    {

    }
}
