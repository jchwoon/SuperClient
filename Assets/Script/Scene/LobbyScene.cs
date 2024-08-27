using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
    protected override void Awake()
    {
        base.Awake();

        SendReqHeroListPacket();
    }

    private void SendReqHeroListPacket()
    {
        ReqHeroListToS reqHeroList = new ReqHeroListToS();
        reqHeroList.AccountId = Utils.GetAccountId();
        Managers.NetworkManager.Send(reqHeroList);
    }

    public void OnReceiveServerData(ResHeroListToC packet)
    {
        if (packet.Lobbyheros.Count == 0)
        {
            Managers.UIManager.ShowSceneUI<CreateHeroSceneUI>("CreateHeroScene");
        }
        else
        {

        }
        //lobbyData.SetHeroInfo(lobbyData.LobbyHeroInfos);
    }
}
