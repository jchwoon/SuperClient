using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Google.Protobuf.Enum;
public class CreateHeroSceneUI : SceneUI
{
    enum Buttons
    {
        BackBtn,
        CreateBtn,
    }
    enum Texts
    {
        Placeholder
    }
    enum GameObjects
    {
        Nickname
    }

    TMP_InputField _nicknameField;
    TMP_Text _placeHolder;

    protected override void Awake()
    {
        base.Awake();

        Bind<Button>(typeof(Buttons));
        Bind<TMP_Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        _nicknameField = Get<GameObject>((int)GameObjects.Nickname).GetComponent<TMP_InputField>();
        Get<TMP_Text>((int)Texts.Placeholder).text = "2~8 사이 닉네임";

        BindEvent(Get<Button>((int)Buttons.BackBtn).gameObject, OnBackBtnClicked);
        BindEvent(Get<Button>((int)Buttons.CreateBtn).gameObject, OnCreateBtnClicked);
    }

    private void OnBackBtnClicked(PointerEventData eventData)
    {
        Managers.SoundManager.PlayClick();
        ShowAndClose();
    }

    private void OnCreateBtnClicked(PointerEventData eventData)
    {
        Managers.SoundManager.PlayClick();
        ReqCreateHeroToS reqCreateHeroPacket = new ReqCreateHeroToS();
        reqCreateHeroPacket.Nickname = _nicknameField.text;
        //Temp
        reqCreateHeroPacket.ClassType = EHeroClassType.Guardian;
        Managers.NetworkManager.Send(reqCreateHeroPacket);
        Clear();
    }

    private void Clear()
    {
        _nicknameField.text = "";
    }
    private void ShowAndClose()
    {
        Managers.UIManager.ShowSceneUI<LobbySceneUI>();
        Managers.UIManager.CloseSceneUI<CreateHeroSceneUI>();
    }

    #region Network
    public void OnReceiveCreateHero(ResCreateHeroToC packet)
    {
        Google.Protobuf.Enum.ECreateHeroResult result = packet.Result;
        switch (result)
        {
            case Google.Protobuf.Enum.ECreateHeroResult.Success:
                //로비 인포 정보를 다시 req하고 응답이 오면 close
                LobbyScene lobby = (LobbyScene)Managers.SceneManagerEx.CurrentScene;
                lobby.SendReqHeroListPacket();
                break;
            case Google.Protobuf.Enum.ECreateHeroResult.FailMinmax:
                Managers.UIManager.ShowAlertPopup("닉네임의 형식이 올바르지 않습니다.", Enums.AlertBtnNum.One);
                break;
            case Google.Protobuf.Enum.ECreateHeroResult.FailOverlap:
                Managers.UIManager.ShowAlertPopup("중복된 닉네임입니다.", Enums.AlertBtnNum.One);
                break;
            default:
                Managers.UIManager.ShowAlertPopup("알 수 없는 오류", Enums.AlertBtnNum.One);
                break;
        }
    }
    #endregion
}
