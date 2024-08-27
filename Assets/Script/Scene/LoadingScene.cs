using Google.Protobuf;
using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
        _loadRoutine = StartCoroutine(SceneLoad());

    }

    IEnumerator SceneLoad()
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync((int)_nextScene);
        asyncOp.allowSceneActivation = false;

        _statusTxt.text = "�� ���� �ҷ����� ��...";
        while (asyncOp.isDone == false)
        {
            //����Ƽ���� allowSceneActivation false�� ��� 0.9����
            float progress = asyncOp.progress / 0.9f;
            _loadingBar.value = progress;

            if (asyncOp.progress >= 0.9f)
            {
                _statusTxt.text = "�ε� �Ϸ�";
                _loadingBar.value = 1.0f;
                asyncOp.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
