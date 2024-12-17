using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using static Enums;

public class CameraController : MonoBehaviour
{
    public GameObject Target { get; set; }
    [Header("Dist")]
    public float targetToDist = 12.0f;
    //Field Of View
    public float minFOV = 20.0f;
    public float maxFOV = 50.0f;
    [Header("Rotate Angle")]
    //플레이어 중심 카메라 X축 회전 각
    public float minXAngle = 20.0f;
    public float maxXAngle = 55.0f;
    //Speed
    [Header("Speed")]
    public float rotateSpeed = 15.0f;
    public float zoomSpeed = 3.0f;
    public float smoothTime = 0.2f;

    Vector3 _velocity = Vector3.zero;
    int _wallLayerMask;

    Camera _mainCam;
    Camera _uiCam;

    int _touchCount = 0;
    //Zoom In/Out
    float _prevDist = 0.0f;
    float _currentDist = 0.0f;

    Action<int> _onChangeTouchCount;
    Coroutine _currentActiveRoutine;
    HashSet<int> _touchIds = new HashSet<int>(3);
    Dictionary<int, Func<ETouchProperty, Vector2>> _getterTouchPosOrDelta = new Dictionary<int, Func<ETouchProperty, Vector2>>()
    {
        {0, touchType => Managers.InputManager.GetFirstTouchInfo(touchType)},
        {1, touchType => Managers.InputManager.GetSecondTouchInfo(touchType)},
        {2, touchType => Managers.InputManager.GetThirdTouchInfo(touchType)},
    };

    private void Start()
    {
        _wallLayerMask = 1 << (int)Enums.Layers.Wall;
        _mainCam = Camera.main;
        _uiCam = Utils.FindChild<Camera>(gameObject);
    }

    private void OnEnable()
    {
        _onChangeTouchCount = ControlByTouchCount;
        Managers.InputManager.StartTouchEvent += OnScreenStartTouch;
        Managers.InputManager.CanceledTouchEvent += OnScreenCanceledTouch;
    }

    private void OnDisable()
    {
        Managers.InputManager.StartTouchEvent -= OnScreenStartTouch;
        Managers.InputManager.CanceledTouchEvent -= OnScreenCanceledTouch;
    }

    #region Camera Target 추적
    private void LateUpdate()
    {
        if (Target == null) return;

        Vector3 targetPos = Target.transform.position;
        Vector3 dir = (transform.position - targetPos).normalized;

        Vector3 camPos = new Vector3(targetPos.x, transform.position.y, targetPos.z) + (new Vector3(dir.x, 0, dir.z) * targetToDist);

        RaycastHit hit;
        Vector3 rayPos = targetPos + (new Vector3(dir.x, dir.y, dir.z) * targetToDist);
        if (Physics.Raycast(targetPos, rayPos - targetPos, out hit, targetToDist, _wallLayerMask) == true)
        {
            camPos = GetHitPos(hit, targetPos);
        }

        transform.position = Vector3.SmoothDamp(transform.position, camPos, ref _velocity, smoothTime);
        transform.LookAt(targetPos);
    }

    private Vector3 GetHitPos(RaycastHit hit, Vector3 targetPos)
    {
        Vector3 camPos = Vector3.zero;
        Vector3 dir = hit.point - targetPos;

        float dist = dir.magnitude * 0.8f;

        camPos = targetPos + dir.normalized * dist;

        return camPos;
    }
    #endregion

    #region 사용자 입력에 따른 제어(이벤트 처리)

    private void OnScreenStartTouch(int touchId, Vector2 touchPos)
    {
        if (IsTouchBlockedUI(touchPos) == false)
        {
            _touchIds.Add(touchId);
            _onChangeTouchCount?.Invoke(++_touchCount);
        }
    }

    private void OnScreenCanceledTouch(int touchId, Vector2 touchPos)
    {
        if (_touchIds.Contains(touchId))
        {
            _touchIds.Remove(touchId);
            _onChangeTouchCount?.Invoke(--_touchCount);
        }
    }

    private void ControlByTouchCount(int touchCount)
    {
        StopCurrentActiveRoutine();
        if (touchCount == 1 && IsValidTouchCount(1))
        {
            _currentActiveRoutine = StartCoroutine(CoCamRotateFromHero());
        }
        else if (touchCount == 2 && IsValidTouchCount(2))
        {
            _currentActiveRoutine = StartCoroutine(CoCamZoomInOutFromHero());
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

    private void StopCurrentActiveRoutine()
    {
        if (_currentActiveRoutine != null)
        {
            StopCoroutine(_currentActiveRoutine);
            _currentActiveRoutine = null;
        }
        Clear();
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

    private bool IsValidTouchCount(int expectedCount)
    {
        return Target != null && _touchIds.Count == expectedCount;
    }

    private void Clear()
    {
        _currentDist = 0;
        _prevDist = 0;
    }
    #endregion

    #region Camera Zoom In / Out
    private void ZoomInOut()
    {
        int[] touchIds = new int[2];
        _touchIds.CopyTo(touchIds);

        Vector2 firstTouchPos = _getterTouchPosOrDelta[touchIds[0]].Invoke(ETouchProperty.Position);
        Vector2 secondTouchPos = _getterTouchPosOrDelta[touchIds[1]].Invoke(ETouchProperty.Position);

        _currentDist = Vector2.Distance(firstTouchPos, secondTouchPos);
        if (_prevDist == 0)
        {
            _prevDist = _currentDist;
            return;
        }

        float power = (_prevDist - _currentDist) * Time.deltaTime * zoomSpeed;

        _mainCam.fieldOfView = Mathf.Clamp(_mainCam.fieldOfView + power, minFOV, maxFOV);
        _uiCam.fieldOfView = _mainCam.fieldOfView;

        _prevDist = _currentDist;
    }
    #endregion

    #region Camera 타겟 중심 회전
    private void RotateAround()
    {
        if (Target == null) return;

        int[] touchId = new int[1];
        _touchIds.CopyTo(touchId);
        Vector2 touchDelta = _getterTouchPosOrDelta[touchId[0]].Invoke(ETouchProperty.Delta);

        float rotX = touchDelta.x * rotateSpeed;
        float rotY = -touchDelta.y * rotateSpeed;

        Transform cam = _mainCam.transform;
        Vector3 currentEulerAngles = cam.eulerAngles;

        float targetAngle = currentEulerAngles.x + rotY * Time.deltaTime;
        float clampedAngle = Mathf.Clamp(targetAngle, minXAngle, maxXAngle);

        Vector3 targetPos = Target.transform.position;

        cam.RotateAround(targetPos, cam.up, rotX * Time.deltaTime);
        cam.RotateAround(targetPos, cam.right, clampedAngle - currentEulerAngles.x);
    }
    #endregion
}
