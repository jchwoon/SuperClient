using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Google.Protobuf.Enum;

public class FootstepController : MonoBehaviour 
{
    TerrainDetector _terrainDetector;
    [SerializeField]
    AudioClip[] _grassClips;
    [SerializeField]
    AudioClip[] _tileClips;

    Hero _hero;

    float _stepDelayTime = 2.0f;
    float _process = 0.0f;

    private void OnEnable()
    {
        _hero = transform.parent.GetComponent<Hero>();
    }

    private void Update()
    {
        if (_hero == null || _hero.Machine == null)
            return;

        float currentSpeed = _hero.Stat.GetStat(EStatType.MoveSpeed);
        float time = _stepDelayTime / currentSpeed;
        if (_hero.Machine.CreatureState == ECreatureState.Move && _process > time)
        {
            Step();
            _process = 0.0f;
        }

        _process += Time.deltaTime;
    }

    private AudioClip GetStepClipForTerrain()
    {
        if (_terrainDetector == null)
        {
            _terrainDetector = new TerrainDetector();
        }
        int terrainIdx = _terrainDetector.GetActiveTerrainTextureIdx(transform.position);

        switch (terrainIdx)
        {
            case 0:
                return _grassClips[Random.Range(0, _grassClips.Length)];
            case 1:
                return _tileClips[Random.Range(0, _grassClips.Length)];
            case 2:
                return null;
            default:
                return null;
        }
    }

    public void Step()
    {
        AudioClip clip = GetStepClipForTerrain();
        if (clip == null)
            return;
        Managers.SoundManager.PlaySFX(clip, transform);
    }
}
