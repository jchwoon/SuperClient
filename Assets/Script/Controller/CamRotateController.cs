using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using static Enums;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class CamRotateController : MonoBehaviour
{
    Transform _target;
    Transform _mainCam;
    CameraController _cc;

    int _touchCount = 0;
    Vector3 velocity = Vector3.zero;
    float prevDist = 0.0f;
    float currentDist = 0.0f;

    Action<int> OnChangeTouchCount;
    Coroutine _currentActiveRoutine;

    HashSet<int> _touchIndexes = new HashSet<int>(3);
    Dictionary<int, Func<ETouchProperty, Vector2>> _getterTouchPosOrDelta = new Dictionary<int, Func<ETouchProperty, Vector2>>()
    {
        {0, touchType => Managers.InputManager.GetFirstTouchInfo(touchType)},
        {1, touchType => Managers.InputManager.GetSecondTouchInfo(touchType)},
        {2, touchType => Managers.InputManager.GetThirdTouchInfo(touchType)},
    };
    private void Start()
    {
        _mainCam = Camera.main.transform;
        _cc = _mainCam.GetComponent<CameraController>();
    }

    private void OnEnable()
    {
        OnChangeTouchCount = ControlByTouchCount;
        Managers.InputManager.StartTouchEvent += OnScreenStartTouch;
        Managers.InputManager.CanceledTouchEvent += OnScreenCanceledTouch;
    }

    private void OnDisable()
    {
        Managers.InputManager.StartTouchEvent -= OnScreenStartTouch;
        Managers.InputManager.CanceledTouchEvent -= OnScreenCanceledTouch;
    }
        
    private void OnScreenStartTouch(int touchIdx, Vector2 touchPos)
    {
        if (IsTouchBlockedUI(touchPos) == false)
        {
            _touchIndexes.Add(touchIdx);
            OnChangeTouchCount?.Invoke(++_touchCount);
        }
    }

    private void OnScreenCanceledTouch(int touchIdx, Vector2 touchPos)
    {
        if (_touchIndexes.Contains(touchIdx))
        {
            _touchIndexes.Remove(touchIdx);
            OnChangeTouchCount?.Invoke(--_touchCount);
        }
    }

    private void ControlByTouchCount(int touchCount)
    {
        StopCurrentActiveRoutine();
        switch (touchCount)
        {
            case 1:
                _currentActiveRoutine = StartCoroutine(CoCamRotateFromHero());
                return;
            case 2:
                _currentActiveRoutine = StartCoroutine(CoCamZoomInOutFromHero());
                return;
        }
    }

    IEnumerator CoCamRotateFromHero()
    {
        while (true)
        {
            RotateAround();
            yield return null;
        }
    }

    IEnumerator CoCamZoomInOutFromHero()
    {
        while (true)
        {
            ZoomInOut();
            yield return null;
        }
    }
    private void ZoomInOut()
    {
        //두 터치의 위치를 먼저 구하고 거리 구하기
        _target = _cc.TargetTransform;
        if (_target == null)
            return;

        if (_touchIndexes.Count != 2)
            return;

        int[] touchIds = new int[2];
        _touchIndexes.CopyTo(touchIds);

        Vector2 firstTouchPos = _getterTouchPosOrDelta[touchIds[0]].Invoke(ETouchProperty.Position);
        Vector2 secondTouchPos = _getterTouchPosOrDelta[touchIds[1]].Invoke(ETouchProperty.Position);

        currentDist = Vector2.Distance(firstTouchPos, secondTouchPos);
        //줌 인
        if (currentDist > prevDist)
        {
            Vector3 dir = (_target.position - _mainCam.position).normalized;
            Vector3 targetCamPos = _mainCam.position + dir * Time.deltaTime;
            if (Vector3.Distance(targetCamPos, _target.position) < _cc.minDistance) return;
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position,
                targetCamPos, ref velocity, 0.1f);
        }
        //줌 아웃
        else
        {
            Vector3 dir = (_mainCam.position - _target.position).normalized;
            Vector3 targetCamPos = _mainCam.position + dir * Time.deltaTime;
            if (Vector3.Distance(targetCamPos, _target.position) < _cc.maxDistance) return;
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position,
                targetCamPos, ref velocity, 0.1f);
        }

        prevDist = currentDist;
    }

    private void RotateAround()
    {
        _target = _cc.TargetTransform;
        if (_target == null)
            return;

        if (_touchIndexes.Count != 1)
            return;

        Vector2 touchDelta = Vector2.zero;
        foreach (int touchIdx in _touchIndexes)
        {
            touchDelta = _getterTouchPosOrDelta[touchIdx].Invoke(ETouchProperty.Delta);
        }
        
        float rotX = touchDelta.x * 25;
        float rotY = -touchDelta.y * 25;

        Transform cam = Camera.main.transform;

        cam.RotateAround(_target.transform.position, cam.up, rotX * Time.deltaTime);
        cam.RotateAround(_target.transform.position, cam.right, rotY * Time.deltaTime);
    }

    private void StopCurrentActiveRoutine()
    {
        if (_currentActiveRoutine != null)
        {
            StopCoroutine(_currentActiveRoutine);
            _currentActiveRoutine = null;
        }
    }
    private bool IsTouchBlockedUI(Vector2 touchPos)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = touchPos
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);
        return results.Count > 0;
    }
}
