using Google.Protobuf.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoySceneUI : SceneUI
{
    enum GameObjects
    {
        Movestick,
    }

    enum Buttons
    {
        AttackBtn
    }
    JoyMoveController _joyMoveController;
    protected override void Awake()
    {
        base.Awake();

        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));

        GameObject movestick = Get<GameObject>((int)GameObjects.Movestick);
        Button attackBtn = Get<Button>((int)Buttons.AttackBtn);
        _joyMoveController = movestick.GetComponent<JoyMoveController>();


        BindEvent(attackBtn.gameObject, OnAttackBtnClicked);
        BindEvent(movestick, OnMovestickPointerDown, Enums.TouchEvent.PointerDown);
        BindEvent(movestick, OnMovestickPointerUp, Enums.TouchEvent.PointerUp);
        BindEvent(movestick, OnMovestickDrag, Enums.TouchEvent.Drag);
    }

    private void OnMovestickPointerDown(PointerEventData eventData)
    {
        _joyMoveController.OnHandlePointerDown(eventData);
    }

    private void OnMovestickPointerUp(PointerEventData eventData)
    {
        _joyMoveController.OnHandlePointerUp(eventData);
    }

    private void OnMovestickDrag(PointerEventData eventData)
    {
        _joyMoveController.OnHandleDrag(eventData);
    }

    private void OnAttackBtnClicked(PointerEventData eventData)
    {
        Managers.EventBus.InvokeEvent(Enums.EventType.AtkBtnClick);
    }
}
