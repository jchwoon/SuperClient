using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController
{
    Renderer _renderer;
    Material[] _originalMats;
    Material _outlineMat;
    GameObject _targetMark;
    bool _isTargetted = false;
    BaseObject _target;

    public TargetController(BaseObject target)
    {
        _target = target;
        _outlineMat = Managers.ResourceManager.GetResource<Material>("TargetOutline");
    }

    public void OnTarget()
    {
        if (_isTargetted)
            return;
        _isTargetted = true;
        AddTargetMark();
        AddOutline();
    }

    public void ClearTarget()
    {
        if (_isTargetted == false)
            return;
        _isTargetted = false;
        RemoveTargetMark();
        RemoveOutline();
    }

    private void AddOutline()
    {
        Renderer renderer = Utils.FindChild<Renderer>(_target.gameObject);
        if (renderer == null)
            return;
        _originalMats = renderer.materials;
        Material[] mats = renderer.materials;
        Array.Resize(ref mats, mats.Length + 1);
        mats[mats.Length - 1] = _outlineMat;
        renderer.materials = mats;
    }

    private void RemoveOutline()
    {
        if (_target == null || _renderer == null)
            return;

        _renderer.materials = _originalMats;
    }

    private void AddTargetMark()
    {
        if (_targetMark == null)
            _targetMark = Managers.ResourceManager.Instantiate("TargetMark", _target.transform);

        _targetMark.SetActive(true);
    }

    private void RemoveTargetMark()
    {
        _targetMark.SetActive(false);
    }
}
