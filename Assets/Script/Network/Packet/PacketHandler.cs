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
        ResCreateHeroToC resCreateHeroPacket = (ResCreateHeroToC)packet;
        CreateHeroSceneUI ui = Managers.UIManager.ShowSceneUI<CreateHeroSceneUI>();
        if (ui != null)
            ui.OnReceiveServerData(resCreateHeroPacket);
    }

    public static void ResDeleteHeroToCHandler(PacketSession session, IMessage packet)
    {
        ResDeleteHeroToC resDeleteHeroPacket = (ResDeleteHeroToC)packet;
        LobbySceneUI ui = Managers.UIManager.ShowSceneUI<LobbySceneUI>();
        if (ui != null)
            ui.OnReceiveServerData(resDeleteHeroPacket);
    }
}
