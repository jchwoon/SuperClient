using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Enums;

public class SceneManagerEx
{
    public SceneType CurrentScene { get; private set; }
    public void ChangeScene(SceneType sceneType)
    {
        SceneManager.LoadScene((int)sceneType);

        CurrentScene = sceneType;
    }
}
