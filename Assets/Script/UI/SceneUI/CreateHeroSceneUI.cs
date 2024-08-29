using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreateHeroSceneUI : SceneUI
{
    enum Buttons
    {
        BackBtn,
        CreateBtn,
    }
    enum Texts
    {
        NicknameTxt
    }

    TMP_Text _nicknameTxt;

    protected override void Awake()
    {
        base.Awake();

        Bind<Button>(typeof(Buttons));
        Bind<TMP_Text>(typeof(Texts));

        _nicknameTxt = Get<TMP_Text>((int)Texts.NicknameTxt);

        BindEvent(Get<Button>((int)Buttons.BackBtn).gameObject, OnBackBtnClicked);
        BindEvent(Get<Button>((int)Buttons.CreateBtn).gameObject, OnCreateBtnClicked);
    }

    private void OnBackBtnClicked(PointerEventData eventData)
    {
        Managers.UIManager.CloseSceneUI<CreateHeroSceneUI>();
    }

    private void OnCreateBtnClicked(PointerEventData eventData)
    {

        ReqCreateHeroToS reqCreateHeroPacket = new ReqCreateHeroToS();
        reqCreateHeroPacket.Nickname = _nicknameTxt.text;
        reqCreateHeroPacket.ClassType = Google.Protobuf.Enum.EHeroClassType.Warrior;
        Managers.NetworkManager.Send(reqCreateHeroPacket);
    }

    public void OnReceiveServerData(ResCreateHeroToC packet)
    {
        Google.Protobuf.Enum.ECreateHeroResult result = packet.Result;
        switch (result)
        {
            case Google.Protobuf.Enum.ECreateHeroResult.Success:
                //로비 인포 정보를 다시 req하고 응답이 오면 close
                LobbyScene lobby = (LobbyScene)Managers.SceneManagerEx.CurrentScene;
                lobby.SendReqHeroListPacket(()=>
                {
                    Managers.UIManager.CloseSceneUI<CreateHeroSceneUI>();
                });
                break;
            case Google.Protobuf.Enum.ECreateHeroResult.FailMinmax:
                Debug.Log("닉네임 최소 최대 안맞음");
                break;
            case Google.Protobuf.Enum.ECreateHeroResult.FailOverlap:
                Debug.Log("닉네임 중복");
                break;
            default:
                break;
        }
    }
}
