using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;
using static Unity.VisualScripting.Member;

public class SoundManager
{
    //BGM사운드가 서로 믹스 되면서 자연스럽게 변환하기 위해 2개
    AudioSource _bgmSource;
    AudioSource _bgmSource2;

    Enums.ESoundsType _currentBgmSource;
    Coroutine _crossFadeRoutine;
    AudioSource _sfxSource;
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    GameObject _soundRoot = null;

    const int SFX_SIZE = 10;

    public float bgmVolume = 0.1f;
    public float effectVolume = 0.1f;



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

        if (_bgmSource.clip == clip)
            return;

        if (_bgmSource.isPlaying)
        {
            _bgmSource.Stop();
        }
        if (_crossFadeRoutine != null)
        {
            CoroutineHelper.Instance.StopHelperCoroutine(_crossFadeRoutine);
        }
        _crossFadeRoutine = CoroutineHelper.Instance.StartHelperCoroutine(CoCrossFadeBGM());

        ChangeCurrentBgmSource();

        _bgmSource.clip = clip;
        _bgmSource.Play();
    }

    public void PlaySFX(string key)
    {
        AudioClip clip = LoadAudioClip(key);

        if (clip == null)
            return;

        _sfxSource.PlayOneShot(clip);
    }

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

    private void ChangeCurrentBgmSource()
    {
        if (_currentBgmSource == Enums.ESoundsType.BGM)
        {
            _currentBgmSource = Enums.ESoundsType.BGM2;
        }
        else
        {
            _currentBgmSource = Enums.ESoundsType.BGM;
        }
    }

    IEnumerator CoCrossFadeBGM()
    {
        AudioSource currentBgmSource = null;
        if (_currentBgmSource == Enums.ESoundsType.BGM)
            currentBgmSource = _bgmSource2;
        else
            currentBgmSource = _bgmSource;



        yield return null;
    }

    public void SetBGMVolume(float volume)
    {
        _bgmSource.volume = volume;
        bgmVolume = volume;
    }

    public void SetEffectVolume(float volume)
    {
        effectVolume = volume;
    }
}
