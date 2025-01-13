using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public struct ParticleInfo
{
    public float Duration;
    public string PrefabName;
    public Transform Parent;

    public ParticleInfo(string prefabName, Transform parent, float duration)
    {
        Duration = duration;
        PrefabName = prefabName;
        Parent = parent;
    }
}
public class ParticleController : MonoBehaviour
{
    ParticleSystem _ps;
    ParticleInfo _info;
    Coroutine _particleLifeRoutine;
    float _duration;
    private void Awake()
    {
        _ps = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        _ps.Play();
    }

    private void OnDisable()
    {
        if (_particleLifeRoutine != null)
        {
            StopCoroutine(_particleLifeRoutine);
        }
    }

    public void SetInfo(ParticleInfo info)
    {
        _info = info;
        if (info.Duration == 0)
        {
            _duration = _ps.main.duration;
        }
        else
        {
            _duration = info.Duration;
        }
        SetPos(info);

        _particleLifeRoutine = StartCoroutine(CoDestroyParticle(_duration));
    }

    private void SetPos(ParticleInfo info)
    {
        InitTransform();
    }

    IEnumerator CoDestroyParticle(float duration)
    {
        float process = 0.0f;
        try
        {
            while (process < duration)
            {
                process += Time.deltaTime;
                yield return null;
            }
        }
        finally
        {
            Managers.ResourceManager.Destroy(gameObject, isPool: true);
        }
    }

    private void InitTransform()
    {
        Vector3 euler = transform.localRotation.eulerAngles;
        euler.y = 0;
        transform.localRotation = Quaternion.Euler(euler);
        transform.localPosition = new Vector3(0, transform.position.y, 0);
    }
}
