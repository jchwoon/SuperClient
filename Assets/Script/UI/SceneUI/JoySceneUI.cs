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
        AttackBtn
    }

    enum Buttons
    {

    }
    JoyMoveController _joyMoveController;
    Image _atkBtnImg;
    [SerializeField]
    Color AtkActivationColor = Color.white;
    [SerializeField]
    Color AtkDeActivationColor = Color.white;
    protected override void Awake()
    {
        base.Awake();

        Bind<GameObject>(typeof(GameObjects));

        GameObject movestick = Get<GameObject>((int)GameObjects.Movestick);
        GameObject atkBtn = Get<GameObject>((int)GameObjects.AttackBtn);
        _atkBtnImg = atkBtn.GetComponent<Image>();
        _joyMoveController = movestick.GetComponent<JoyMoveController>();


        BindEvent(atkBtn, OnAttackBtnClicked);
        BindEvent(movestick, OnMovestickPointerDown, Enums.TouchEvent.PointerDown);
        BindEvent(movestick, OnMovestickPointerUp, Enums.TouchEvent.PointerUp);
        BindEvent(movestick, OnMovestickDrag, Enums.TouchEvent.Drag);
    }

    public void ChangeAtkBtnActivation(bool attacking)
    {
        _atkBtnImg.color = (attacking ? AtkActivationColor : AtkDeActivationColor);
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
