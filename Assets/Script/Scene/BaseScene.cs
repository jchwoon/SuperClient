using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseScene : MonoBehaviour
{
    protected virtual void Awake()
    {
        Managers.SceneManagerEx.CurrentScene = this;
    }
}
