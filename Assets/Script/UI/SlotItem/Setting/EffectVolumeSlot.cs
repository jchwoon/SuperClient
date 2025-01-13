using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EffectVolumeSlot : VolumeSlot
{
    private float _sliderValue = 0f;

    protected override void OnChangedSliderValue(float value)
    {
        base.OnChangedSliderValue(value);

        _sliderValue = value;
        Managers.SoundManager.SetEffectVolume((int)value / SoundManager.MAX_VOLUME_VALUE);
    }

    protected override void OnEnable()
    {
        int value = Managers.SoundManager.GetEffectVolume();
        SetVolume(value);
    }

    protected override void OnDisable()
    {
        GameSettings.SetSound(Enums.ESoundsType.Effect, (int)_sliderValue);
    }
}
