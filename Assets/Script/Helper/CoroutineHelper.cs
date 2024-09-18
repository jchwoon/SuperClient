using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoroutineHelper : MonoBehaviour
{
    private static CoroutineHelper _instance;
    public static CoroutineHelper Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("CoroutineHelper");
                _instance = go.AddComponent<CoroutineHelper>();
                DontDestroyOnLoad(go);
            }

            return _instance;
        }
    }

    public Coroutine StartHelperCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }

    public void StopHelperCoroutine(Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }
}
