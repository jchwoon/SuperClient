using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameSceneUI : SceneUI
{
    enum GameObjects
    {
        BackBtn
    }
    Button _toLobbyBtn;
    protected override void Awake()
    {
        base.Awake();
        Bind<GameObject>(typeof(GameObjects));

        BindEvent(Get<GameObject>((int)GameObjects.BackBtn), OnBackBtnClicked);
    }

    public void SetUI()
    {
        //_fadeEffect.GetComponent<FadeEffect>().FadeInOut();
        //_fadeEffect.GetComponent<Image>().raycastTarget = false;
    }

    private void OnBackBtnClicked(PointerEventData eventData)
    {
        Managers.UIManager.ShowAlertPopup("로비로 이동하시겠습니까?", Enums.AlertBtnNum.Two,
            () => 
            {
                Managers.SceneManagerEx.ChangeScene(Enums.SceneType.Lobby);
                Managers.GameManager.LeaveGame();
            });
    }
}
