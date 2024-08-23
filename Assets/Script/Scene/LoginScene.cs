using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScene : BaseScene
{
    protected override void Awake()
    {
        base.Awake();

        Managers.ResourceManager.LoadAllAsync<Object>("login", (key, currentCount, totalCount) =>
        {
            if (currentCount == totalCount)
            {
                Managers.ResourceManager.Instantiate("LoginCanvas");
            }
        });

    }
}
