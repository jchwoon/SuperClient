using Data;
using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Awake()
    {
        base.Awake();
        ReqEnterRoomToS reqEnterPacket = new ReqEnterRoomToS();
        reqEnterPacket.HeroIdx = Managers.GameManager.SelectHeroIdx;
        Managers.NetworkManager.Send(reqEnterPacket);
    }
    public void OnReceiveEnterRoom(ResEnterRoomToC packet)
    {
        RoomData room;
        if (Managers.DataManager.RoomDict.TryGetValue(packet.MyHero.HeroInfo.CreatureInfo.ObjectInfo.RoomId, out room) == false)
            return;

        string key = room.Name;
        Managers.MapManager.LoadMap(key);
        //hero setting
        Managers.ObjectManager.Spawn(packet.MyHero);
        //ui setting
        Managers.UIManager.ShowSceneUI<GameSceneUI>()?.SetUI();

        //Todo fadein
    }

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
    }
}
