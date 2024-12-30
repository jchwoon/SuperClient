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
        ToSelectSceneBtn,
        SkillBtn
    }
    protected override void Awake()
    {
        base.Awake();

        Bind<GameObject>(typeof(GameObjects));

        BindEvent(Get<GameObject>((int)GameObjects.DungeonBtn), OnDungeonBtnClicked);
        BindEvent(Get<GameObject>((int)GameObjects.CloseBtn), OnCloseBtnClicked);
        BindEvent(Get<GameObject>((int)GameObjects.ToSelectSceneBtn), OnToSelectSceneBtnClicked);
        BindEvent(Get<GameObject>((int)GameObjects.SkillBtn), OnSkillBtnClicked);
    }

    private void OnDungeonBtnClicked(PointerEventData eventData)
    {
        Managers.SoundManager.PlayClick();
        ClosePopup();
        Managers.UIManager.ShowPopup<DungeonUI>();
    }
    private void OnCloseBtnClicked(PointerEventData eventData)
    {
        Managers.SoundManager.PlayClick();
        ClosePopup();
    }
    private void OnToSelectSceneBtnClicked(PointerEventData eventData)
    {
        Managers.SoundManager.PlayClick();
        Managers.UIManager.ShowAlertPopup("로비로 이동하시겠습니까?", Enums.AlertBtnNum.Two,
        () =>
        {
            Managers.SceneManagerEx.ChangeScene(Enums.SceneType.Lobby);
            Managers.GameManager.LeaveGame();
        });
    }

    private void OnSkillBtnClicked(PointerEventData eventData)
    {
        Managers.SoundManager.PlayClick();
        ClosePopup();
        SkillUI skillUI = Managers.UIManager.ShowPopup<SkillUI>();
        skillUI.Refresh();
    }

    private void ClosePopup()
    {
        ClosePopup<MenuUI>();
    }
}
