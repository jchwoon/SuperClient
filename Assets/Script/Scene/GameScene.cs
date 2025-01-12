using Data;
using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
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
        Managers.NetworkManager.Send(reqEnterPacket);
    }

    public void OnReceiveEnterRoom(ResEnterRoomToC packet)
    {        
        //Temp
        PlayBGM("VillageBGM");
        Managers.MapManager.CreateMap();
        Managers.UIManager.ShowFadeUI();

        //게임에 첫 입장
        if (packet.IsChangeRoom == false)
        {
            Managers.ObjectManager.Spawn(packet.MyHero, (MyHero myHero) =>
            {
                myHero.Inventory.InitInventory(packet.Items.ToList());
                myHero.SkillComponent.InitSkill(packet.Skills.ToDictionary(kvp => kvp.SkillId, kvp => kvp.SkillLevel));
                myHero.SkillComponent.InitSkillPoint(packet.SkillPoint);

                GameSceneUI gSceneUI = Managers.UIManager.ShowSceneUI<GameSceneUI>();
                gSceneUI.Refresh();
                Managers.UIManager.ShowSceneUI<JoySceneUI>();
            });
        }
    }

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
    }
}
