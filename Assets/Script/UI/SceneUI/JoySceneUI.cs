using Google.Protobuf.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class JoySceneUI : SceneUI
{
    enum GameObjects
    {
        Joystick,
        JoyHandle
    }

    enum Buttons
    {
        AttackBtn
    }

    RectTransform _joystickRect;
    RectTransform _joyHandleRect;
    private bool _isPress;
    private Vector2 _moveInput = Vector2.zero;

    protected override void Awake()
    {
        base.Awake();

        Bind<GameObject>(typeof(GameObjects));
        Bind<UnityEngine.UI.Button>(typeof(Buttons));

        GameObject joystick = Get<GameObject>((int)GameObjects.Joystick);
        GameObject joyHandle = Get<GameObject>((int)GameObjects.JoyHandle);

        _joystickRect = joystick.GetComponent<RectTransform>();
        _joyHandleRect = joyHandle.GetComponent<RectTransform>();

        BindEvent(joystick, OnHandlePointerDown, Enums.TouchEvent.PointerDown);
        BindEvent(joystick, OnHandlePointerUp, Enums.TouchEvent.PointerUp);
        BindEvent(joystick, OnHandleDrag, Enums.TouchEvent.Drag);
    }

    protected override void Update()
    {
        if (_isPress == false)
            return;
        Managers.GameManager.MoveInput = _moveInput;
    }

    private void OnHandlePointerDown(PointerEventData eventData)
    {
        SetJoyPos(eventData);
        _isPress = true;
    }

    private void OnHandlePointerUp(PointerEventData eventData)
    {
        Managers.GameManager.MoveInput = Vector2.zero;
        _joyHandleRect.anchoredPosition = Vector2.zero;
        _isPress = false;
    }

    private void OnHandleDrag(PointerEventData eventData)
    {
        SetJoyPos(eventData);
    }

    private void SetJoyPos(PointerEventData eventData)
    {
        Vector2 touchPos = Vector2.zero;
        bool inner = RectTransformUtility.ScreenPointToLocalPointInRectangle(_joystickRect, eventData.position, eventData.pressEventCamera, out touchPos);
        if (inner == true)
        {
            //[-0.5, 0.5]
            touchPos = touchPos / _joystickRect.sizeDelta;
            //[-1, 1]
            touchPos *= 2;
            float dist = Mathf.Min(touchPos.magnitude, 1);
            Vector2 handleDir = touchPos.normalized;
            touchPos = handleDir * dist;
            UpdateMoveInput(touchPos);
        }
        _joyHandleRect.anchoredPosition = touchPos * (_joystickRect.anchoredPosition * 0.5f);
    }

    private void UpdateMoveInput(Vector2 input)
    {
        _moveInput = input;
    }

}
