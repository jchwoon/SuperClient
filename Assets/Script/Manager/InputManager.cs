using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager
{
    TouchController touchController;

    public event Action<Vector2> PerformedTabAction;

    public void Awake()
    {
        touchController = new TouchController();
    }
    public void OnEnable()
    {
        touchController.Enable();
    }

    public void OnDisable()
    {
        touchController.Disable();
    }

    public void Start()
    {
        touchController.Touch.TouchPress.performed += (context) => PerformedTab(context);
    }

    public Vector2 GetTouchPosition()
    {
        return touchController.Touch.TouchPosition.ReadValue<Vector2>();
    }

    private void PerformedTab(InputAction.CallbackContext context)
    {
        if (PerformedTabAction == null)
            return;

        PerformedTabAction.Invoke(GetTouchPosition());
    }
}
