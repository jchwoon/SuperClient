using Google.Protobuf.Enum;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    TextMeshPro _text;
    Vector3 spawnRange = new Vector3(0.5f, 0, 0);
    [SerializeField]
    Color _normalHitFontColor;
    [SerializeField]
    Color _healFontColor;
    [SerializeField]
    Color _goldFontColor;
    [SerializeField]
    Color _expFontColor;

    private void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }

    private void OnDisable()
    {
        _text.alpha = 0.0f;
    }

    public void SetInfo(float value, EFontType fontType)
    {
        _text = GetComponent<TextMeshPro>();
        transform.localPosition = Vector3.up;
        _text.alpha = 1.0f;
        _text.sortingOrder = 600;

        switch (fontType)
        {
            case EFontType.NormalHit:
                _text.fontSize = 5;
                _text.color = _normalHitFontColor;
                _text.text = $"{Mathf.Abs((int)value)}";
                break;
            case EFontType.Heal:
                _text.fontSize = 5;
                _text.color = _healFontColor;
                _text.text = $"{Mathf.Abs((int)value)}";
                break;
            case EFontType.Gold:
                _text.fontSize = 5;
                _text.text = $"Gold +{(int)value}";
                _text.color = _goldFontColor;
                break;
            case EFontType.Exp:
                _text.fontSize = 5;
                _text.text = $"Exp +{(int)value}";
                _text.color = _expFontColor;
                break;
            default:
                Debug.Log("Non Type Floating Text");
                break;
        }
    }

    //애니메이션 이벤트
    public void OnCompleteAnimation()
    {
        Managers.ResourceManager.Destroy(gameObject, isPool: true);
    }
}
