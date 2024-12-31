using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemVolumeSlot : VolumeSlot
{
    private float _sliderValue = 0f;
    protected override void OnChangedSliderValue(float value)
    {
        base.OnChangedSliderValue(value);
        _sliderValue = value;
        Debug.Log(value / SoundManager.MAX_VOLUME_VALUE);
        Managers.SoundManager.SetSystemVolume(value / SoundManager.MAX_VOLUME_VALUE);
    }

    protected override void OnEnable()
    {
        int value = Managers.SoundManager.GetSystemVolume();
        SetVolume(value);
    }

    protected override void OnDisable()
    {
        GameSettings.SetSound(Enums.ESoundsType.System, (int)_sliderValue);
    }
}
