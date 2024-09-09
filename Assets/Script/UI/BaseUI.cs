using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public abstract class BaseUI : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();
    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
    }

    protected virtual void OnEnable()
    {
    }

    protected virtual void Update()
    {

    }

    protected void Bind<T>(Type enumType) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(enumType);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Utils.FindChild(gameObject, names[i], true);
            else
                objects[i] = Utils.FindChild<T>(gameObject, names[i], true);
            if (objects[i] == null)
                Debug.Log($"Failed to bind({names[i]})");
        }
    }

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == true)
            return objects[idx] as T;

        return null;
    }

    public void BindEvent(GameObject go, Action<PointerEventData> action = null, Enums.TouchEvent type = Enums.TouchEvent.Click)
    {
        UIEventHandler handler = go.GetComponent<UIEventHandler>();
        if (handler == null)
            handler = go.AddComponent<UIEventHandler>();

        switch (type)
        {
            case Enums.TouchEvent.Click:
                handler.OnClickHandler -= action;
                handler.OnClickHandler += action;
                break;
            case Enums.TouchEvent.PointerDown:
                handler.OnPointerDownHandler -= action;
                handler.OnPointerDownHandler += action;
                break;
            case Enums.TouchEvent.PointerUp:
                handler.OnPointerUpHandler -= action;
                handler.OnPointerUpHandler += action;
                break;
            case Enums.TouchEvent.Drag:
                handler.OnDragHandler -= action;
                handler.OnDragHandler += action;
                break;
            case Enums.TouchEvent.BeginDrag:
                handler.OnBeginDragHandler -= action;
                handler.OnBeginDragHandler += action;
                break;
            case Enums.TouchEvent.EndDrag:
                handler.OnEndDragHandler -= action;
                handler.OnEndDragHandler += action;
                break;
        }
    }
}
