using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour
{
    private static Managers _instance;

    private static Managers Instance
    {
        get { MainInit(); return _instance; }
    }

    private NetworkManager _networkManager = new NetworkManager();
    private SceneManagerEx _sceneManager = new SceneManagerEx();
    private ResourceManager _resourceManager = new ResourceManager();
    private UIManager _uiManager = new UIManager();
    private DataManager _dataManager = new DataManager();
    private GameManager _gameManager = new GameManager();
    private ObjectManager _objectManagr = new ObjectManager();

    public static NetworkManager NetworkManager { get { return Instance._networkManager; } }
    public static SceneManagerEx SceneManagerEx { get { return Instance._sceneManager; } }
    public static ResourceManager ResourceManager { get { return Instance._resourceManager; } }
    public static UIManager UIManager { get { return Instance._uiManager; } }
    public static DataManager DataManager { get { return Instance._dataManager; } }
    public static GameManager GameManager { get { return Instance._gameManager; } }
    public static ObjectManager ObjectManager { get { return Instance._objectManagr; } }

    private void Awake()
    {
        MainInit();

    }
    private static void MainInit()
    {
        if (_instance == null)
        {
            GameObject gameObject = GameObject.Find("Managers");
            if (gameObject == null)
            {
                gameObject = new GameObject("Managers");
                gameObject.AddComponent<Managers>();
            }
            DontDestroyOnLoad(gameObject);

            _instance = gameObject.GetComponent<Managers>();
        }
    }

    private void Update()
    {
        _networkManager.Update();
    }
}
