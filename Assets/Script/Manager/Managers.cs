using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour
{
    private static Managers s_instance;

    private static Managers Instance
    {
        get { MainInit(); return s_instance; }
    }

    private NetworkManager _networkManager = new NetworkManager();

    public static NetworkManager NetworkManager { get { return Instance._networkManager; } }

    private void Awake()
    {
        MainInit();
    }
    private static void MainInit()
    {
        if (s_instance == null)
        {
            GameObject gameObject = GameObject.Find("Managers");
            if (gameObject == null)
            {
                gameObject = new GameObject("Managers");
                gameObject.AddComponent<Managers>();
            }
            DontDestroyOnLoad(gameObject);

            s_instance = gameObject.GetComponent<Managers>();
        }
    }
}
