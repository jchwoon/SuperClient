using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    Queue<GameObject> _poolQueue = new Queue<GameObject>();
    public Transform Parent { get; private set; }
    public Pool(string goName)
    {
        GameObject go = new GameObject($"{goName}Pool");
        Parent = go.transform;
    }

    public void Push(GameObject go)
    {
        go.SetActive(false);
        go.transform.SetParent(Parent);
        _poolQueue.Enqueue(go);
    }

    public GameObject Pop (Transform parent)
    {
        if (_poolQueue.Count == 0)
            return null;

        GameObject go = _poolQueue.Dequeue();
        go.SetActive(true);
        go.transform.SetParent(parent);
        return go;
    }
}

public class PoolManager
{
    Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();

    public bool Push(GameObject go)
    {
        if (go == null)
            return false;

        if (_pools.ContainsKey(go.name) == false)
        {
            Pool pool = CreatePool(go.name);

            pool.Push(go);
            return true;
        }

        _pools[go.name].Push(go);
        return true;
    }

    public GameObject Pop(GameObject go, Transform parent)
    {
        if (_pools.ContainsKey(go.name) == false)
        {
            CreatePool(go.name);
            return null;
        }

        
        return _pools[go.name].Pop(parent);
    }
    public void Clear()
    {
        _pools.Clear();
    }
    private Pool CreatePool(string key)
    {
        Pool newPool = new Pool(key);
        _pools.Add(key, newPool);

        return newPool;
    }
}
