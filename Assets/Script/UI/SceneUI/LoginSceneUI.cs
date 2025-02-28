using Data;
using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using System.Net;
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

    TMP_Text _startTxt;
    TMP_Text _stateTxt;
    Button _startBtn;
    bool _isStartState;

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
        if (CheckStartState() == false)
            return;

        Managers.AuthManager.RequestLogin();

        //Managers.SoundManager.PlayClick();
        //Managers.ResourceManager.LoadAllAsync<Object>("preLoad", (key, currentCount, totalCount) =>
        //{
        //    _stateTxt.text = $"데이타 로딩중... : {key} {currentCount} / {totalCount}";
        //    if (currentCount == totalCount)
        //    {
        //        _stateTxt.text = "데이타 로드 완료";
        //        OnDataLoaded();
        //    }
        //});
    }

    private void OnDataLoaded()
    {
        Managers.DataManager.Init();
        _stateTxt.text = $"게임 서버에 연결 중...";

        //Local
        if (Managers.DataManager.ConfigDict.TryGetValue(Enums.EConfigIds.Local, out ConfigData configData) == false)
            return;
        IPAddress ipAddress = IPAddress.Parse(configData.Ip);

        //Remote
        //if (Managers.DataManager.ConfigDict.TryGetValue(Enums.EConfigIds.Remote, out ConfigData configData) == false)
        //    return;
        //IPAddress ipAddress = Dns.GetHostAddresses(configData.Ip)[0];
        IPEndPoint endPoint = new IPEndPoint(ipAddress, configData.Port);

        Managers.NetworkManager.OnConnectedAction = OnConnected;
        Managers.NetworkManager.OnFailedAction = OnFailed;
        Managers.NetworkManager.Connect(endPoint);
    }

    private void OnConnected()
    {
        Managers.SceneManagerEx.ChangeScene(Enums.SceneType.Lobby);
        Clear();
    }
    private void OnFailed()
    {
        Clear();
        //
    }

    private bool CheckStartState()
    {
        if (_isStartState == true)
            return false;
        _isStartState = true;
        _startBtn.interactable = false;
        return true;
    }
    private void Clear()
    {
        _isStartState = false;
        _startBtn.interactable = true;
    }
}
