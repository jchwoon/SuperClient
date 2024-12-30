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

    AudioSource _sfxSource;

    Enums.ESoundsType _currentBgmSource;
    Coroutine _crossFadeRoutine;

    const float CROSS_FADE_DURATION = 2.0f;
    const int SFX_SIZE = 10;

    GameObject _soundRoot = null;

    //public float bgmVolume = 0.1f;
    //public float effectVolume = 0.1f;



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

                //SFX
                GameObject sfxGo = new GameObject($"SFX");
                sfxGo.transform.parent = _soundRoot.transform;
                _sfxSource = sfxGo.AddComponent<AudioSource>();
            }
        }
    }

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

    //2D SFX
    public void PlaySFX(string key)
    {
        AudioClip clip = LoadAudioClip(key);

        if (clip == null)
            return;

        _sfxSource.PlayOneShot(clip);
    }
    //3D SFX
    public void PlaySFX(string key, Transform playPos = null)
    {
        AudioClip clip = LoadAudioClip(key);

        if (clip == null)
            return;

        GameObject go = Managers.ResourceManager.Instantiate("SfxSource", isPool: true);

        if (go == null)
            return;

        AudioSource audioSource = Utils.GetOrAddComponent<AudioSource>(go);

        go.transform.position = playPos.position;
        audioSource.clip = clip;
        float clipLength = audioSource.clip.length;

        audioSource.Play();

        CoroutineHelper.Instance.StartHelperCoroutine(CoReserveDestroySource(go, clipLength));
    }

    public void PlaySFX(AudioClip clip, Transform playPos = null)
    {
        if (clip == null)
            return;

        GameObject go = Managers.ResourceManager.Instantiate("SfxSource", isPool: true);

        if (go == null)
            return;

        AudioSource audioSource = Utils.GetOrAddComponent<AudioSource>(go);

        go.transform.position = playPos.position;
        audioSource.clip = clip;
        float clipLength = audioSource.clip.length;

        audioSource.Play();

        CoroutineHelper.Instance.StartHelperCoroutine(CoReserveDestroySource(go, clipLength));
    }

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
        //Temp
        float targetVolume = 1.0f;

        try
        {
            while (process < CROSS_FADE_DURATION)
            {
                currentBgmSource.volume -= (currentVolume / CROSS_FADE_DURATION) * Time.deltaTime;
                changedBgmSource.volume += (targetVolume / CROSS_FADE_DURATION) * Time.deltaTime;
                process += Time.deltaTime;
                yield return null;
            }
        }
        finally
        {
            currentBgmSource.volume = 0.0f;
            changedBgmSource.volume = targetVolume;
            currentBgmSource.Stop();
        }
    }

    IEnumerator CoReserveDestroySource(GameObject go, float length)
    {
        yield return new WaitForSeconds(length);
        Managers.ResourceManager.Destroy(go, isPool: true);
    }

    //public void SetBGMVolume(float volume)
    //{
    //    _bgmSource.volume = volume;
    //    bgmVolume = volume;
    //}

    //public void SetEffectVolume(float volume)
    //{
    //    effectVolume = volume;
    //}
}
