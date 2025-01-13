using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmVolumeSlot : VolumeSlot
{
    private float _sliderValue = 0f;
    protected override void OnChangedSliderValue(float value)
    {
        base.OnChangedSliderValue(value);

        _sliderValue = value;
        Managers.SoundManager.SetBGMVolume((int)value / SoundManager.MAX_VOLUME_VALUE);
    }

    protected override void OnEnable()
    {
        int value = Managers.SoundManager.GetBGMVolume();
        SetVolume(value);
    }

    protected override void OnDisable()
    {
        GameSettings.SetSound(Enums.ESoundsType.BGM, (int)_sliderValue);
    }
}
