using Data;
using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Awake()
    {
        base.Awake();

        ReqEnterRoomToS reqEnterPacket = new ReqEnterRoomToS();
        reqEnterPacket.HeroIdx = Managers.GameManager.SelectHeroIdx;
        Managers.MapManager.CreateMap();
        Managers.NetworkManager.Send(reqEnterPacket);
    }

    public void OnReceiveEnterRoom(ResEnterRoomToC packet)
    {
        if (packet.IsChangeRoom == false)
        {
            Managers.UIManager.ShowSceneUI<GameSceneUI>();
            Managers.UIManager.ShowSceneUI<JoySceneUI>();
            Managers.ObjectManager.Spawn(packet.MyHero, (MyHero myHero) =>
            {
                myHero.Inventory.InitInventory(packet.Items.ToList());
            });
        }

        Managers.UIManager.ShowFadeUI();
    }

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
    }
}
