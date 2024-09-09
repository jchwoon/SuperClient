using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Enums;

public class SceneManagerEx
{
    private BaseScene _currentScene;
    private SceneType _nextScene = SceneType.None;
    public BaseScene CurrentScene 
    { 
        get { return  _currentScene; }
        set { _currentScene = value; }
    }
    public SceneType NextScene
    {
        get { return _nextScene; }
    }
    public void ChangeScene(SceneType sceneType)
    {
        _nextScene = sceneType;
        SceneManager.LoadScene((int)SceneType.Loading);
    }
}
