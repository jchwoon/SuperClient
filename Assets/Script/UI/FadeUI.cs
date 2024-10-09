using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : BaseUI
{
    enum Images
    {
        FadeEffect
    }
    Image _fadeImg;
    Coroutine _fadeCoroutine;
    [SerializeField]
    float _fadeTime;

    protected override void Awake()
    {
        Bind<Image>(typeof(Images));

        _fadeImg = Get<Image>((int)Images.FadeEffect);
    }

    public void FadeInOut(bool isFadeIn = true)
    {
        if (isFadeIn == true)
        {
            _fadeImg.raycastTarget = false;
            _fadeCoroutine = StartCoroutine(FadeRoutine(1, 0));
        }
        else
            _fadeCoroutine = StartCoroutine(FadeRoutine(0, 1));
    }

    IEnumerator FadeRoutine(float start, float end)
    {
        Color color = _fadeImg.color;
        float currentTime = 0.0f;
        float process = 0.0f;
        while (process < 1)
        {
            currentTime += Time.deltaTime;
            process = currentTime / _fadeTime;

            color.a = Mathf.Lerp(start, end, process);
            _fadeImg.color = color;

            yield return null;
        }
    }
}
