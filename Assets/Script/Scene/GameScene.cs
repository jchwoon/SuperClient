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
        Debug.Log(packet.MyHero);
        //hero setting
        Managers.ObjectManager.Spawn(packet.MyHero);
        //ui setting
        Managers.UIManager.ShowSceneUI<GameSceneUI>()?.SetUI();
    }

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
    }
}
