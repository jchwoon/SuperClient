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
        lobbyScene.OnReceiveHeroList(heroListPacket);
    }
    public static void ResCreateHeroToCHandler(PacketSession session, IMessage packet)
    {
        ResCreateHeroToC resCreateHeroPacket = (ResCreateHeroToC)packet;
        CreateHeroSceneUI ui = Managers.UIManager.ShowSceneUI<CreateHeroSceneUI>();
        if (ui != null)
            ui.OnReceiveCreateHero(resCreateHeroPacket);
    }

    public static void ResDeleteHeroToCHandler(PacketSession session, IMessage packet)
    {
        ResDeleteHeroToC resDeleteHeroPacket = (ResDeleteHeroToC)packet;
        LobbySceneUI ui = Managers.UIManager.ShowSceneUI<LobbySceneUI>();
        if (ui != null)
            ui.OnReceiveDeleteHero(resDeleteHeroPacket);
    }
    public static void ResEnterRoomToCHandler(PacketSession session, IMessage packet)
    {
        ResEnterRoomToC resEnterPacket = (ResEnterRoomToC)packet;
        GameScene gameScene = (GameScene)Managers.SceneManagerEx.CurrentScene;
        gameScene.OnReceiveEnterRoom(resEnterPacket);
    }
}
