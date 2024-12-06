using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CamRotateController : MonoBehaviour
{
    bool _isTouchBg = false;
    Vector2 _prevPos = Vector2.zero;
    Transform _target;
    CameraController _cc;
    private void Start()
    {
        _cc = Camera.main.GetComponent<CameraController>();
    }

    private void Update()
    {
        if (Input.touchCount <= 0)
            return;

        if (Input.touchCount == 1)
            RotateAroundHero(Input.GetTouch(0));
        else if (Input.touchCount == 2)
        {
            Touch touch = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId) == true
                && EventSystem.current.IsPointerOverGameObject(touch2.fingerId) == false)
            {
                RotateAroundHero(touch2);
                return;
            }
            else if (EventSystem.current.IsPointerOverGameObject(touch.fingerId) == false
                && EventSystem.current.IsPointerOverGameObject(touch2.fingerId) == true)
            {
                RotateAroundHero(touch);
                return;
            }
        }
    }//적어도 하나는 UI를 터치했을때만

    private void RotateAroundHero(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
        {
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                _isTouchBg = false;
                return;
            }
            else
            {
                _prevPos = touch.deltaPosition;
                _isTouchBg = true;
            }
        }

        if (_isTouchBg == true)
        {
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 delta = touch.deltaPosition;
                Vector2 dir = (_prevPos - delta).normalized;
                LookAround(dir);
            }

            if (touch.phase == TouchPhase.Ended)
            {
                _isTouchBg = false;
            }
        }

    }


    private void LookAround(Vector2 dir)
    {
        _target = _cc.TargetTransform;
        if (_target == null)
            return;

        Transform cam = Camera.main.transform;
        Vector3 camAngle = cam.eulerAngles;
        Vector3 targetToCamDir = cam.position - _target.position;

        float ratio = Mathf.Abs(targetToCamDir.x) + Mathf.Abs(targetToCamDir.z);
        ratio = Mathf.Clamp(ratio, 2.0f, ratio);

        cam.RotateAround(_target.transform.position, cam.up, -dir.x * Time.deltaTime * 6 * ratio);
        cam.RotateAround(_target.transform.position, cam.right, dir.y * Time.deltaTime * 6 * ratio);
    }
}
