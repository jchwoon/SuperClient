using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToastUI : BaseUI
{
    enum Texts
    {
        ToastTxt
    }
    Coroutine _fadeRoutine;
    TMP_Text _toastTxt;
    CanvasGroup _canvasGroup;
    float _fadeDuration = 1.0f;

    protected override void Awake()
    {
        base.Awake();
        Bind<TMP_Text>(typeof(Texts));

        _toastTxt = Get<TMP_Text>((int)Texts.ToastTxt);
        _canvasGroup = gameObject.GetComponent<CanvasGroup>();

        _canvasGroup.alpha = 0;
    }

    public void ShowToast(string text, float duration)
    {
        _toastTxt.text = text;
        ForceStopToast();
        _fadeRoutine = StartCoroutine(FadeInOut(duration, _fadeDuration));
    }
    IEnumerator FadeInOut(float duration, float fadeDuration)
    {
        yield return Fade(0, 1, fadeDuration);
        yield return new WaitForSeconds(duration);
        yield return Fade(1, 0, fadeDuration);
    }

    IEnumerator Fade(float start, float end, float duration)
    {
        float currentTime = 0.0f;
        float process = 0.0f;
        while (process < 1)
        {
            currentTime += Time.deltaTime;
            process = currentTime / duration;

            _canvasGroup.alpha = Mathf.Lerp(start, end, process);
            yield return null;
        }
    }

    private void ForceStopToast()
    {
        _canvasGroup.alpha = 0;
        if (_fadeRoutine != null)
            StopCoroutine(_fadeRoutine);
    }
}
