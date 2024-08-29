using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
    Image _fadeImg;
    Coroutine _fadeCoroutine;
    [SerializeField]
    float _fadeTime;

    bool _isFadeIn;

    private void Awake()
    {
        _fadeImg = GetComponent<Image>();
    }

    public void FadeInOut(bool isFadeIn = true)
    {
        _isFadeIn = isFadeIn;
        if (_isFadeIn == true)
            _fadeCoroutine = StartCoroutine(FadeInOut(1, 0));
        else
            _fadeCoroutine = StartCoroutine(FadeInOut(0, 1));
    }

    IEnumerator FadeInOut(float start, float end)
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
