using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBusManager
{
    Dictionary<Enums.EventType, Action> _events = new Dictionary<Enums.EventType, Action>();

    public void AddEvent(Enums.EventType type, Action action)
    {
        if (_events.ContainsKey(type) == false)
            _events.Add(type, action);
        else
            _events[type] += action;
    }

    public void RemoveEvent(Enums.EventType type, Action action)
    {
        if (_events.ContainsKey(type) == true)
            _events[type] -= action;
    }

    public void InvokeEvent(Enums.EventType type)
    {
        if (_events.ContainsKey(type) == true)
            _events[type].Invoke();
    }
}
