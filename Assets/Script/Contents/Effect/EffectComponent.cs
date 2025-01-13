using Data;
using Google.Protobuf.Enum;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectComponent
{
    public Creature Owner;

    Dictionary<long, GameObject> _effectParticles = new Dictionary<long, GameObject>();

    public EffectComponent(Creature owner)
    {
        Owner = owner;
    }

    public void ApplyEffect(EffectData effectData, long effectId)
    {
        if (effectData == null)
            return;

        if (string.IsNullOrEmpty(effectData.PrefabName))
            return;

        ParticleInfo particleInfo = new ParticleInfo
        (
            effectData.PrefabName,
            Owner.transform,
            effectData.Duration
        );

        ParticleController controller = Managers.ObjectManager.SpawnParticle(particleInfo);
        _effectParticles.Add(effectId, controller.gameObject);
    }

    public void ReleaseEffect(long effectId)
    {
        if (_effectParticles.TryGetValue(effectId, out GameObject go))
        {
            _effectParticles.Remove(effectId);
            Managers.ResourceManager.Destroy(go, isPool: true);
        }
    }
}
