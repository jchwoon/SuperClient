using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LobbyScene : BaseScene
{
    private Action _receiveHeroListAction;
    protected override void Awake()
    {
        base.Awake();

        SendReqHeroListPacket();
    }

    public void SendReqHeroListPacket(Action action = null)
    {
        _receiveHeroListAction = action;
        ReqHeroListToS reqHeroList = new ReqHeroListToS();
        reqHeroList.AccountId = Utils.GetAccountId();
        Managers.NetworkManager.Send(reqHeroList);
    }

    public void OnReceiveServerData(ResHeroListToC packet)
    {
        _receiveHeroListAction?.Invoke();
        if (packet.Lobbyheros.Count == 0)
        {
            Managers.UIManager.ShowSceneUI<CreateHeroSceneUI>();
        }
        else
        {
            LobbySceneUI lobby = Managers.UIManager.ShowSceneUI<LobbySceneUI>();
            lobby.SetCharacterSlotInfo(packet.Lobbyheros.ToList());
        }
        //lobbyData.SetHeroInfo(lobbyData.LobbyHeroInfos);
    }
}
