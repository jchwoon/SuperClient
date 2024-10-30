using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetOutlineController : MonoBehaviour
{
    Renderer _renderer;
    Material[] _originalMats;
    [SerializeField]
    Material _outlineMat;

    public void AddOutline(Creature target)
    {
        if (target == null)
            return;

        Renderer renderer = target.transform.Find("Renderer").GetComponent<Renderer>();
        _originalMats = renderer.materials;
        Material[] mats = renderer.materials;
        Array.Resize(ref mats, mats.Length + 1);
        mats[mats.Length - 1] = _outlineMat;
        renderer.materials = mats;
    }

    public void BackToOriginMats(Creature target)
    {
        if (target == null)
            return;

        Renderer renderer = target.transform.Find("Renderer").GetComponent<Renderer>();
        renderer.materials = _originalMats;
    }
}
