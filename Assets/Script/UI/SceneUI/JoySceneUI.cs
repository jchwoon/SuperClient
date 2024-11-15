using Data;
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
        AttackBtn,
        PickUpBtn
    }

    JoyMoveController _joyMoveController;
    JoyPickupController _joyPickupController;
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
        GameObject pickUpBtn = Get<GameObject>((int)GameObjects.PickUpBtn);
        _atkBtnImg = atkBtn.GetComponent<Image>();
        _joyMoveController = movestick.GetComponent<JoyMoveController>();
        _joyPickupController = pickUpBtn.GetComponent<JoyPickupController>();

        BindEvent(atkBtn, OnAttackBtnClicked);
        BindEvent(movestick, OnMovestickPointerDown, Enums.TouchEvent.PointerDown);
        BindEvent(movestick, OnMovestickPointerUp, Enums.TouchEvent.PointerUp);
        BindEvent(movestick, OnMovestickDrag, Enums.TouchEvent.Drag);
        BindEvent(pickUpBtn, OnPickUpBtnClicked);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        Managers.EventBus.AddEvent(Enums.EventType.ChangeAttackMode, ChangeAtkBtnActivation);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        Managers.EventBus.RemoveEvent(Enums.EventType.ChangeAttackMode, ChangeAtkBtnActivation);
    }

    public void ChangeAtkBtnActivation()
    {
        MyHero hero = Managers.ObjectManager.MyHero;
        if (hero == null)
            return;

        bool attacking = hero.MyHeroStateMachine.AttackMode;

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

    private void OnPickUpBtnClicked(PointerEventData eventData)
    {
        _joyPickupController.OnHandlePickupClick();
    }
}
