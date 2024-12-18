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
    // Start is called before the first frame update
    void Start()
    {
        _ps = GetComponent<ParticleSystem>();
    }

    public void SetInfo(ParticleInfo info)
    {
        _info = info;
        SetPos(info);
        if (_particleLifeRoutine != null)
        {
            StopCoroutine(_particleLifeRoutine);
        }
        _particleLifeRoutine = StartCoroutine(CoDestroyParticle(info.Duration));
    }

    private void SetPos(ParticleInfo info)
    {
        InitTransform();
    }
    IEnumerator CoDestroyParticle(float duration)
    {
        float coolTime = duration;
        float process = 0.0f;
        while (process < coolTime)
        {
            process += Time.deltaTime;
            yield return null;
        }
        Managers.ResourceManager.Destroy(gameObject, isPool : true);
    }

    private void InitTransform()
    {
        Vector3 euler = transform.localRotation.eulerAngles;
        euler.y = 0;
        transform.localRotation = Quaternion.Euler(euler);
        transform.localPosition = Vector3.zero;
    }
}
