using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyMoveController : MonoBehaviour
{
    RectTransform _joyMoveRect;
    RectTransform _joyMoveHandleRect;
    //private bool _isPress;
    float _camRotY;
    Vector2 _prevInput = Vector2.zero;
    private void Start()
    {
        _joyMoveRect = GetComponent<RectTransform>();
        _joyMoveHandleRect = transform.GetChild(0).GetComponent<RectTransform>();
    }

    public void OnHandlePointerDown(PointerEventData eventData)
    {
        SetJoyPos(eventData);
        //_isPress = true;
    }
    public void OnHandlePointerUp(PointerEventData eventData)
    {
        Managers.GameManager.MoveInput = Vector2.zero;
        _joyMoveHandleRect.anchoredPosition = Vector2.zero;
        //_isPress = false;
    }
    public void OnHandleDrag(PointerEventData eventData)
    {
        SetJoyPos(eventData);
    }

    private void SetJoyPos(PointerEventData eventData)
    {
        Vector2 touchPos = Vector2.zero;
        bool inner = RectTransformUtility.ScreenPointToLocalPointInRectangle(_joyMoveRect, eventData.position, eventData.pressEventCamera, out touchPos);
        if (inner == true)
        {
            //[-0.5, 0.5]
            touchPos = touchPos / _joyMoveRect.sizeDelta;
            //[-1, 1]
            touchPos *= 2;
            float dist = Mathf.Min(touchPos.magnitude, 1);
            Vector2 handleDir = touchPos.normalized;
            touchPos = handleDir * dist;
            SetMoveInput(touchPos);
        }
        _joyMoveHandleRect.anchoredPosition = touchPos * (_joyMoveRect.sizeDelta / 2);
    }

    private void SetMoveInput(Vector2 dir)
    {
        Vector3 camDir = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * new Vector3(dir.x, 0, dir.y);
        Vector2 input = new Vector2(camDir.x, camDir.z);
        Managers.GameManager.MoveInput = input;
    }
}
