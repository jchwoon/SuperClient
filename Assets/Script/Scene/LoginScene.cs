using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScene : BaseScene
{
    protected override void Awake()
    {
        base.Awake();

        Managers.ResourceManager.LoadAllAsync<Object>("title", (key, currentCount, totalCount) =>
        {
            if (currentCount == totalCount)
            {
                PlayBGM("TitleBGM");
            }
        });

    }

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
    }
}
