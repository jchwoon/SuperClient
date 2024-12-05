using Data;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : BaseScene
{
    [SerializeField]
    Slider _loadingBar;
    [SerializeField]
    TMP_Text _statusTxt;

    Coroutine _loadRoutine;
    Enums.SceneType _nextScene;
    protected override void Awake()
    {
        base.Awake();
        _nextScene = Managers.SceneManagerEx.NextScene;

        if (_nextScene == Enums.SceneType.Game)
        {
            PreEnterRoomToS preEnterPacket = new PreEnterRoomToS();
            preEnterPacket.HeroIdx = Managers.GameManager.SelectHeroIdx;
            Managers.NetworkManager.Send(preEnterPacket);
        }
        else
            _loadRoutine = StartCoroutine(SceneLoad());
    }

    public void OnReceivePreEnterRoom(PreEnterRoomToC packet)
    {
        RoomData room;
        if (Managers.DataManager.RoomDict.TryGetValue(packet.RoomId, out room) == false)
            return;

        string key = room.Name;
        _statusTxt.text = "맵 정보 불러오는 중...";
        Managers.MapManager.LoadMap(key, OnLoadedMap);
    }
    public void OnReceiveChangeRoom(int roomId)
    {
        RoomData room;
        if (Managers.DataManager.RoomDict.TryGetValue(roomId, out room) == false)
            return;

        string key = room.Name;
        _statusTxt.text = "맵 정보 불러오는 중...";
        Managers.MapManager.LoadMap(key, OnLoadedMap);
    }

    private void OnLoadedMap()
    {
        _loadRoutine = StartCoroutine(SceneLoad());
    }

    IEnumerator SceneLoad()
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync((int)_nextScene);
        asyncOp.allowSceneActivation = false;
        _statusTxt.text = "씬 정보 불러오는 중...";


        float t = 0.0f;
        while (asyncOp.isDone == false)
        {
            //유니티에서 allowSceneActivation false인 경우 0.9까지
            float progress = (asyncOp.progress + 1.0f) / 1.9f;
            if (asyncOp.progress < 0.9f)
            {
                _loadingBar.value = progress;
            }
            if (asyncOp.progress >= 0.9f)
            {
                t += Time.deltaTime;
                _loadingBar.value = Mathf.Lerp(0.9f, 1.0f, t);
                if (_loadingBar.value >= 1.0f)
                {
                    _statusTxt.text = "로딩 완료";
                    asyncOp.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
    }
}
