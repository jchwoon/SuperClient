using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VolumeSlot : BaseUI
{
    enum Sliders
    {
        SoundSlider
    }
    enum GameObjects
    {
        MinusBtn,
        PlusBtn
    }
    enum Texts
    {
        RatioValueTxt
    }

    Slider _volumeSlider;

    protected override void Awake()
    {
        Bind<Slider>(typeof(Sliders));
        Bind<GameObject>(typeof(GameObjects));
        Bind<TMP_Text>(typeof(Texts));

        _volumeSlider = Get<Slider>((int)Sliders.SoundSlider);

        _volumeSlider.maxValue = SoundManager.MAX_VOLUME_VALUE;
        _volumeSlider.onValueChanged.AddListener(OnChangedSliderValue);

        BindEvent(Get<GameObject>((int)GameObjects.MinusBtn), OnClickedMinusBtn);
        BindEvent(Get<GameObject>((int)GameObjects.PlusBtn), OnClickedPlusBtn);
    }

    protected void SetVolume(int volume)
    {
        _volumeSlider.value = volume;
        Get<TMP_Text>((int)Texts.RatioValueTxt).text = $"{volume} %";
    }

    protected virtual void OnClickedPlusBtn(PointerEventData eventData)
    {
        _volumeSlider.value += 1;
    }

    protected virtual void OnClickedMinusBtn(PointerEventData eventData)
    {
        _volumeSlider.value -= 1;
    }

    protected virtual void OnChangedSliderValue(float value)
    {
        Get<TMP_Text>((int)Texts.RatioValueTxt).text = $"{((int)value)} %";
    }
}
