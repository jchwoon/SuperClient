using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUI : SceneUI
{
    enum GameObjects
    {
        FadeEffect,
    }
    GameObject _fadeEffect;
    protected override void Awake()
    {
        base.Awake();
        Bind<GameObject>(typeof(GameObjects));

        _fadeEffect = Get<GameObject>((int)GameObjects.FadeEffect);
    }

    public void SetUI()
    {
        //_fadeEffect.GetComponent<FadeEffect>().FadeInOut();
        //_fadeEffect.GetComponent<Image>().raycastTarget = false;
    }
}
