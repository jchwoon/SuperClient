using Google.Protobuf;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CallbackQueue 
{
    private static CallbackQueue _instance;
    public static CallbackQueue Instance
    {
        get
        {
            if (_instance == null)
                _instance = new CallbackQueue();

            return _instance;
        }
    }

    Queue<Action> _actionQueue = new Queue<Action>();
    object _lock = new object();

    public void Push(Action action)
    {
        if (action == null)
            return; 

        lock (_lock)
        {
            _actionQueue.Enqueue(action);
        }
    }

    public List<Action> Pop()
    {
        List<Action> actionList = new();
        lock (_lock)
        {
            while (_actionQueue.Count > 0)
            {
                Action packet = _actionQueue.Dequeue();
                actionList.Add(packet);
            }
        }

        return actionList;
    }
}
