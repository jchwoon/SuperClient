using Google.Protobuf.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TextInfo
{
    public float value;
    public Transform parent;
    public EFontType fontType;
}
public class FloatingTextController : MonoBehaviour
{
    private Queue<TextInfo> _floatingQueue = new Queue<TextInfo>();

    public void OnEnable()
    {
        _floatingQueue.Clear();
        StartCoroutine(CoWaitFloatingText());
    }

    public void RegisterOrSpawnText(float value, Transform parent, EFontType fontType, bool isReward = false)
    {
        if (isReward == true)
        {
            TextInfo info = new TextInfo()
            {
                value = value,
                parent = parent,
                fontType = fontType
            };

            _floatingQueue.Enqueue(info);
            return;
        }

        SpawnFloatingText(value, parent, fontType);
    }

    public void SpawnFloatingText(float value, Transform parent, EFontType fontType)
    {
        GameObject go = Managers.ResourceManager.Instantiate("FloatingText", parent, isPool:true);
        FloatingText text = go.GetComponent<FloatingText>();

        text.SetInfo(value, fontType);
    }

    IEnumerator CoWaitFloatingText()
    {
        WaitForSeconds sec = new WaitForSeconds(0.2f);
        while (true)
        {
            if (_floatingQueue.Count > 0)
            {
                TextInfo info = _floatingQueue.Dequeue();
                SpawnFloatingText(info.value, info.parent, info.fontType);
            }
            yield return sec;
        }
    }
}
