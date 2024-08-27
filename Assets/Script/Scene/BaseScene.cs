using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScene : MonoBehaviour
{
    protected virtual void Awake()
    {
        Managers.SceneManagerEx.CurrentScene = this;
    }
}
