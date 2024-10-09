using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScene : BaseScene
{
    GameObject _fadeEffect;
    private Action _receiveHeroListAction;
    protected override void Awake()
    {
        base.Awake();
        _fadeEffect = Managers.UIManager.Parent.Find("AlertCanvas").Find("FadeEffect").gameObject;
        SendReqHeroListPacket();
    }

    public void SendReqHeroListPacket(Action action = null)
    {
        _receiveHeroListAction = action;
        ReqHeroListToS reqHeroList = new ReqHeroListToS();
        reqHeroList.AccountId = Utils.GetAccountId();
        Managers.NetworkManager.Send(reqHeroList);
    }

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
    }

    #region Receive
    public void OnReceiveHeroList(ResHeroListToC packet)
    {
        Debug.Log(packet.Lobbyheros);
        _receiveHeroListAction?.Invoke();

        _fadeEffect.GetComponent<FadeEffect>().FadeInOut();
        _fadeEffect.GetComponent<Image>().raycastTarget = false;
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
    #endregion
}
