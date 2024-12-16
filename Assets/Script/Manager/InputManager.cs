using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Enums;

public class InputManager
{
    TouchController touchController;

    public event Action<int, Vector2> StartTouchEvent;
    public event Action<int, Vector2> CanceledTouchEvent;

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
        touchController.Touch.FirstTouchPress.started += (_) => OnStartFirstTouch();
        touchController.Touch.FirstTouchPress.canceled += (_) => OnCanceledFirstTouch();

        touchController.Touch.SecondTouchPress.started += (_) => OnStartSecondTouch();
        touchController.Touch.SecondTouchPress.canceled += (_) => OnCanceledSecondTouch();

        touchController.Touch.ThirdTouchPress.started += (_) => OnStartThirdTouch();
        touchController.Touch.ThirdTouchPress.canceled += (_) => OnCanceledThirdTouch();
    }

    private void OnStartFirstTouch()
    {
        if (StartTouchEvent == null)
            return;
        
        StartTouchEvent.Invoke(0, GetFirstTouchInfo(ETouchProperty.Position));
    }
    private void OnStartSecondTouch()
    {
        if (StartTouchEvent == null)
            return;

        StartTouchEvent.Invoke(1, GetSecondTouchInfo(ETouchProperty.Position));
    }
    private void OnStartThirdTouch()
    {
        if (StartTouchEvent == null)
            return;

        StartTouchEvent.Invoke(2, GetThirdTouchInfo(ETouchProperty.Position));
    }

    private void OnCanceledFirstTouch()
    {
        if (CanceledTouchEvent == null)
            return;

        CanceledTouchEvent.Invoke(0, GetFirstTouchInfo(ETouchProperty.Position));
    }
    private void OnCanceledSecondTouch()
    {
        if (CanceledTouchEvent == null)
            return;

        CanceledTouchEvent.Invoke(1, GetSecondTouchInfo(ETouchProperty.Position));
    }
    private void OnCanceledThirdTouch()
    {
        if (CanceledTouchEvent == null)
            return;

        CanceledTouchEvent.Invoke(2, GetThirdTouchInfo(ETouchProperty.Position));
    }

    #region GetTouch Pos / Delta

    public Vector2 GetFirstTouchInfo(ETouchProperty touchType)
    {
        if (touchType == ETouchProperty.Position)
            return touchController.Touch.FirstTouchPosition.ReadValue<Vector2>();
        else
            return touchController.Touch.FirstTouchDelta.ReadValue<Vector2>();
    }
    public Vector2 GetSecondTouchInfo(ETouchProperty touchType)
    {
        if (touchType == ETouchProperty.Position)
            return touchController.Touch.SecondTouchPosition.ReadValue<Vector2>();
        else
            return touchController.Touch.SecondTouchDelta.ReadValue<Vector2>();
    }
    public Vector2 GetThirdTouchInfo(ETouchProperty touchType)
    {
        if (touchType == ETouchProperty.Position)
            return touchController.Touch.ThirdTouchPosition.ReadValue<Vector2>();
        else
            return touchController.Touch.ThirdTouchDelta.ReadValue<Vector2>();
    }
    #endregion
}
