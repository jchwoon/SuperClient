using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuUI : PopupUI
{
    enum GameObjects
    {
        DungeonBtn,
        CloseBtn,
        ToSelectSceneBtn
    }
    protected override void Awake()
    {
        base.Awake();

        Bind<GameObject>(typeof(GameObjects));

        BindEvent(Get<GameObject>((int)GameObjects.DungeonBtn), OnDungeonBtnClicked);
        BindEvent(Get<GameObject>((int)GameObjects.CloseBtn), OnCloseBtnClicked);
        BindEvent(Get<GameObject>((int)GameObjects.ToSelectSceneBtn), OnToSelectSceneBtnClicked);
    }

    private void OnDungeonBtnClicked(PointerEventData eventData)
    {
        Managers.UIManager.ShowPopup<DungeonUI>();
    }
    private void OnCloseBtnClicked(PointerEventData eventData)
    {
        ClosePopup<MenuUI>();
    }
    private void OnToSelectSceneBtnClicked(PointerEventData eventData)
    {
        Managers.UIManager.ShowAlertPopup("로비로 이동하시겠습니까?", Enums.AlertBtnNum.Two,
        () =>
        {
            Managers.SceneManagerEx.ChangeScene(Enums.SceneType.Lobby);
            Managers.GameManager.LeaveGame();
        });
    }
}
