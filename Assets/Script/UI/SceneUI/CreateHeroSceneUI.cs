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
        Placeholder
    }
    enum GameObjects
    {
        Nickname
    }

    [SerializeField]
    AlertUI _alertPopup;
    TMP_InputField _nicknameField;
    TMP_Text _placeHolder;

    protected override void Awake()
    {
        base.Awake();

        Bind<Button>(typeof(Buttons));
        Bind<TMP_Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        _nicknameField = Get<GameObject>((int)GameObjects.Nickname).GetComponent<TMP_InputField>();
        Get<TMP_Text>((int)Texts.Placeholder).text = "2~8 ���� �г���";

        BindEvent(Get<Button>((int)Buttons.BackBtn).gameObject, OnBackBtnClicked);
        BindEvent(Get<Button>((int)Buttons.CreateBtn).gameObject, OnCreateBtnClicked);
    }

    private void OnBackBtnClicked(PointerEventData eventData)
    {
        ShowAndClose();
    }

    private void OnCreateBtnClicked(PointerEventData eventData)
    {

        ReqCreateHeroToS reqCreateHeroPacket = new ReqCreateHeroToS();
        reqCreateHeroPacket.Nickname = _nicknameField.text;
        reqCreateHeroPacket.ClassType = Google.Protobuf.Enum.EHeroClassType.Warrior;
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
                //�κ� ���� ������ �ٽ� req�ϰ� ������ ���� close
                LobbyScene lobby = (LobbyScene)Managers.SceneManagerEx.CurrentScene;
                lobby.SendReqHeroListPacket(() =>
                {
                    ShowAndClose();
                });
                break;
            case Google.Protobuf.Enum.ECreateHeroResult.FailMinmax:
                _alertPopup.gameObject.SetActive(true);
                _alertPopup.SetAlert("�г����� ������ �ùٸ��� �ʽ��ϴ�.", Enums.AlertBtnNum.One);
                break;
            case Google.Protobuf.Enum.ECreateHeroResult.FailOverlap:
                _alertPopup.gameObject.SetActive(true);
                _alertPopup.SetAlert("�ߺ��� �г����Դϴ�.", Enums.AlertBtnNum.One);
                break;
            default:
                _alertPopup.gameObject.SetActive(true);
                _alertPopup.SetAlert("�� �� ���� ����", Enums.AlertBtnNum.One);
                break;
        }
    }
    #endregion
}
