using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ResourceManager
{
    private Dictionary<string, UnityEngine.Object> _resources = new Dictionary<string, UnityEngine.Object>();


    public T GetResource<T> (string key) where T: UnityEngine.Object
    {
        if (_resources.TryGetValue(key, out UnityEngine.Object obj))
        {
            return obj as T;
        }

        return null;
    }

    public GameObject Instantiate (string key, Transform parent = null, bool isPool = false)
    {
        GameObject prefab = GetResource<GameObject>(key);

        if (prefab == null)
            return null;

        if (isPool == true)
        {
            GameObject poolGo = Managers.PoolManager.Pop(prefab, parent);
            if (poolGo != null)
                return poolGo;
        }

        GameObject go = UnityEngine.Object.Instantiate(prefab, parent);

        go.name = prefab.name;
        return go;
    }

    public GameObject Instantiate(GameObject original,  Transform parent = null)
    {
        GameObject go = UnityEngine.Object.Instantiate(original, parent);

        go.name = original.name;
        return go;
    }

    public void Destroy(GameObject go, bool isPool = false)
    {
        if (go == null)
            return;

        if (isPool == true)
        {
            if (Managers.PoolManager.Push(go) == true)
                return;
        }

        UnityEngine.Object.Destroy(go);
    }

    public void LoadAsync<T>(string key, Action<T> action = null) where T : UnityEngine.Object
    {
        string loadKey = key;

        if (typeof(T) == typeof(Sprite))
        {
            loadKey = $"{key}[{key}]";
        }

        AsyncOperationHandle<T> asyncOperation = Addressables.LoadAssetAsync<T>(loadKey);
        asyncOperation.Completed += (op) =>
        {
            if (_resources.TryGetValue(key, out UnityEngine.Object resource))
            {
                action?.Invoke(op.Result);
                return;
            }

            _resources.Add(key, op.Result);
            action?.Invoke(op.Result);
        };
    }
    public void LoadAllAsync<T>(string label, Action<string, int, int> action) where T : UnityEngine.Object
    {
        var opHandle = Addressables.LoadResourceLocationsAsync(label, typeof(T));
        opHandle.Completed += (op) =>
        {
            int loadCount = 0;///
            int totalCount = op.Result.Count;

            foreach (var result in op.Result)
            {
                if (result.ResourceType == typeof(Texture2D) || result.ResourceType == typeof(Sprite))
                {
                    LoadAsync<Sprite>(result.PrimaryKey, (obj) =>
                    {
                        loadCount++;
                        action?.Invoke(result.PrimaryKey, loadCount, totalCount);
                    });
                }
                else
                {
                    LoadAsync<T>(result.PrimaryKey, (obj) =>
                    {
                        loadCount++;
                        action?.Invoke(result.PrimaryKey, loadCount, totalCount);
                    });
                }
            }
        };
    }
}
