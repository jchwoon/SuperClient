using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CameraController : MonoBehaviour
{
    public Transform TargetTransform { get; set; }
    float _targetDist = 12.0f;
    float _smoothTime = 0.3f;
    Vector3 _velocity = Vector3.zero;
    Vector3 TargetPos = Vector3.zero;
    int _wallLayerMask;

    private void Start()
    {
        _wallLayerMask = 1 << (int)Enums.Layers.Wall;
    }

    private void LateUpdate()
    {
        if (TargetTransform == null)
            return;
        TargetPos = TargetTransform.Find("HeadPoint").position;
        Vector3 dir = (transform.position - TargetPos).normalized;

        Vector3 camPos = new Vector3(TargetPos.x, transform.position.y, TargetPos.z) + (new Vector3(dir.x, 0, dir.z) * _targetDist);
        
        RaycastHit hit;
        Vector3 rayPos = TargetPos + (new Vector3(dir.x, dir.y, dir.z) * _targetDist);
        if (Physics.Raycast(TargetPos, rayPos - TargetPos, out hit, _targetDist, _wallLayerMask) == true)
        {
            camPos = GetHitPos(hit);
        }

        transform.position = Vector3.SmoothDamp(transform.position, camPos, ref _velocity, _smoothTime);
        transform.LookAt(TargetPos);
    }

    private Vector3 GetHitPos(RaycastHit hit)
    {
        Vector3 camPos = Vector3.zero;
        Vector3 dir = hit.point - TargetPos;

        float dist = dir.magnitude * 0.8f;

        camPos = TargetPos + dir.normalized * dist;

        return camPos;
    }
}
