using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoginSceneUI : SceneUI
{
    enum Texts
    {
        StartTxt,
        StateTxt
    }

    enum Buttons
    {
        StartBtn
    }

    private TMP_Text _startTxt;
    private TMP_Text _stateTxt;
    private Button _startBtn;

    protected override void Awake()
    {
        base.Awake();
        Bind<TMP_Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        GetData();
        SetData();
    }

    private void GetData()
    {
        _stateTxt = Get<TMP_Text>((int)Texts.StateTxt);
        _startTxt = Get<TMP_Text>((int)Texts.StartTxt);
        _startBtn = Get<Button>((int)Buttons.StartBtn);
    }
    private void SetData()
    {
        _startTxt.text = "Start";
        _stateTxt.text = "";
        BindEvent(_startBtn.gameObject, OnStartBtnClicked);
    }

    private void OnStartBtnClicked(PointerEventData eventData)
    {
        //서버와의 연결이 필요
        Managers.ResourceManager.LoadAllAsync<Object>("preLoad", (key, currentCount, totalCount) =>
        {
            _stateTxt.text = $"데이타 로딩중... : {key} {currentCount} / {totalCount}";
            if (currentCount == totalCount)
            {
                _stateTxt.text = $"데이타 로드 완료";
                OnDataLoaded();
            }
        });


    }

    private void OnDataLoaded()
    {
        _stateTxt.text = $"게임 서버에 연결 중...";
        Managers.NetworkManager.OnConnectedAction = OnConnected;
        Managers.NetworkManager.OnFailedAction = OnFailed;
        Managers.NetworkManager.Connect();

    }

    private void OnConnected()
    {
        Managers.SceneManagerEx.ChangeScene(Enums.SceneType.Lobby);
    }
    private void OnFailed()
    {
        //
    }
}
