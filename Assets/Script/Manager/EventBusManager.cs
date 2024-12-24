using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBusManager
{
    private Dictionary<Enums.EventType, Delegate> _events = new Dictionary<Enums.EventType, Delegate>();

    public void AddEvent(Enums.EventType type, Action action)
    {
        if (_events.ContainsKey(type) == false)
            _events.Add(type, null);

        _events[type] = (Action)_events[type] + action;
    }

    public void AddEvent<T>(Enums.EventType type, Action<T> action)
    {
        if (_events.ContainsKey(type) == false)
            _events.Add(type, null);

        _events[type] = (Action<T>)_events[type] + action;
    }

    public void RemoveEvent(Enums.EventType type, Action action)
    {
        if (_events.ContainsKey(type) == true)
            _events[type] = (Action)_events[type] - action;
    }

    public void RemoveEvent<T>(Enums.EventType type, Action<T> action)
    {
        if (_events.ContainsKey(type) == true)
            _events[type] = (Action<T>)_events[type] - action;
    }

    public void InvokeEvent(Enums.EventType type)
    {
        if (_events.ContainsKey(type) == true)
            (_events[type] as Action)?.Invoke();
    }

    public void InvokeEvent<T>(Enums.EventType type, T param)
    {
        if (_events.ContainsKey(type) == true)
            (_events[type] as Action<T>)?.Invoke(param);
    }
}
