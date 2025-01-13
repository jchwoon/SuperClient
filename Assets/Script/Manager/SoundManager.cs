using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.InputSystem;
using static Unity.VisualScripting.Member;

public class SoundManager
{
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    //BGM사운드가 서로 믹스 되면서 자연스럽게 변환하기 위해 2개
    AudioSource _bgmSource;
    AudioSource _bgmSource2;

    AudioSource _systemSource;

    Enums.ESoundsType _currentBgmSource;
    Coroutine _crossFadeRoutine;

    const float CROSS_FADE_DURATION = 2.0f;
    public static readonly float MAX_VOLUME_VALUE = 100.0f;

    GameObject _soundRoot = null;

    float _effectVolume = 0.1f;

    #region Init
    public void SoundInit()
    {
        if (_soundRoot == null)
        {
            _soundRoot = GameObject.Find("SoundRoot");
            if (_soundRoot == null )
            {
                _soundRoot = new GameObject("SoundRoot");

                UnityEngine.Object.DontDestroyOnLoad(_soundRoot);

                //BGM
                GameObject bgmGo = new GameObject("BGM");
                bgmGo.transform.parent = _soundRoot.transform;
                _bgmSource = bgmGo.AddComponent<AudioSource>();
                _bgmSource.loop = true;

                GameObject bgm2Go = new GameObject("BGM2");
                bgm2Go.transform.parent = _soundRoot.transform;
                _bgmSource2 = bgm2Go.AddComponent<AudioSource>();
                _bgmSource2.loop = true;

                _currentBgmSource = Enums.ESoundsType.BGM;

                float bgmVolume = GameSettings.GetSound(Enums.ESoundsType.BGM);
                _bgmSource.volume = bgmVolume;
                _bgmSource2.volume = bgmVolume;

                //Effect
                GameObject systemGo = new GameObject($"SystemEffect");
                systemGo.transform.parent = _soundRoot.transform;
                _systemSource = systemGo.AddComponent<AudioSource>();
                _systemSource.volume = GameSettings.GetSound(Enums.ESoundsType.System);
                SetEffectVolume(GameSettings.GetSound(Enums.ESoundsType.Effect));
            }
        }
    }
    #endregion


    #region BGM
    public void PlayBGM(string key)
    {
        AudioClip clip = LoadAudioClip(key);

        if (clip == null)
            return;

        if (GetCurrentPlayingBgmSource().clip == clip)
            return;

        if (_crossFadeRoutine != null)
        {
            CoroutineHelper.Instance.StopHelperCoroutine(_crossFadeRoutine);
        }
        _crossFadeRoutine = CoroutineHelper.Instance.StartHelperCoroutine(CoCrossFadeBGM(clip));
    }
    #endregion

    #region SystemEffect 
    //2D Effect
    public void PlaySFX(string key)
    {
        AudioClip clip = LoadAudioClip(key);

        if (clip == null)
            return;

        _systemSource.PlayOneShot(clip);
    }
    #endregion

    #region Effect 3D Sound
    //3D Effect
    public void PlaySFX(string key, Transform playPos = null)
    {
        AudioClip clip = LoadAudioClip(key);
        Play3DEffect(clip, playPos);
    }

    public void PlaySFX(AudioClip clip, Transform playPos = null)
    {
        Play3DEffect(clip, playPos);
    }

    private void Play3DEffect(AudioClip clip, Transform playPos = null)
    {
        if (clip == null)
            return;

        GameObject go = Managers.ResourceManager.Instantiate("SfxSource", isPool: true);

        if (go == null)
            return;

        AudioSource audioSource = Utils.GetOrAddComponent<AudioSource>(go);

        go.transform.position = playPos.position;
        audioSource.clip = clip;
        audioSource.volume = _effectVolume;
        float clipLength = audioSource.clip.length;

        audioSource.Play();

        CoroutineHelper.Instance.StartHelperCoroutine(CoReserveDestroySource(go, clipLength));
    }

    #endregion

    public void PlayClick() { PlaySFX("Clicked"); }
    public void PlayDropped() { PlaySFX("Dropped"); }

    private AudioClip LoadAudioClip(string key)
    {
        AudioClip audioClip = null;
        if (_audioClips.TryGetValue(key, out audioClip))
        {
            return audioClip;
        }

        audioClip = Managers.ResourceManager.GetResource<AudioClip>(key);

        if (!_audioClips.ContainsKey(key))
            _audioClips.Add(key, audioClip);

        return audioClip;
    }

    private AudioSource GetCurrentPlayingBgmSource()
    {
        return _currentBgmSource == Enums.ESoundsType.BGM ? _bgmSource : _bgmSource2;
    }

    IEnumerator CoCrossFadeBGM(AudioClip newClip)
    {
        AudioSource currentBgmSource = null;
        AudioSource changedBgmSource = null;
        if (_currentBgmSource == Enums.ESoundsType.BGM)
        {
            currentBgmSource = _bgmSource;
            changedBgmSource = _bgmSource2;
            _currentBgmSource = Enums.ESoundsType.BGM2;
        }
        else
        {
            currentBgmSource = _bgmSource2;
            changedBgmSource = _bgmSource;
            _currentBgmSource = Enums.ESoundsType.BGM;
        }

        changedBgmSource.clip = newClip;
        changedBgmSource.volume = 0.0f;
        changedBgmSource.Play();

        float process = 0.0f;
        float currentVolume = currentBgmSource.volume;

        try
        {
            while (process < CROSS_FADE_DURATION)
            {
                currentBgmSource.volume -= (currentVolume / CROSS_FADE_DURATION) * Time.deltaTime;
                changedBgmSource.volume += (currentVolume / CROSS_FADE_DURATION) * Time.deltaTime;
                process += Time.deltaTime;
                yield return null;
            }
        }
        finally
        {
            currentBgmSource.volume = 0.0f;
            changedBgmSource.volume = currentVolume;
            currentBgmSource.Stop();
        }
    }

    IEnumerator CoReserveDestroySource(GameObject go, float length)
    {
        yield return new WaitForSeconds(length);
        Managers.ResourceManager.Destroy(go, isPool: true);
    }

    #region Get / Set
    public void SetBGMVolume(float volume)
    {
        _bgmSource.volume = volume;
        _bgmSource2.volume = volume;
    }

    public void SetEffectVolume(float volume)
    {
        _effectVolume = volume;
    }

    public void SetSystemVolume(float volume)
    {
        _systemSource.volume = volume;
    }

    public int GetBGMVolume()
    {
        return (int)(_bgmSource.volume * MAX_VOLUME_VALUE);
    }

    public int GetEffectVolume()
    {
        return (int)(_effectVolume * MAX_VOLUME_VALUE);
    }

    public int GetSystemVolume()
    {
        return  (int)(_systemSource.volume * MAX_VOLUME_VALUE);
    }
    #endregion
}
