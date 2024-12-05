using Data;
using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseScene : MonoBehaviour
{
    protected virtual void Awake()
    {
        Screen.SetResolution(540, 390, false);
        Managers.SceneManagerEx.CurrentScene = this;
    }

    protected virtual void OnApplicationQuit()
    {
        Managers.GameManager.LeaveGame();
        Managers.NetworkManager.Disconnect();
    }

}
