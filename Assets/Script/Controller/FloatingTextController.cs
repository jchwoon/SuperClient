using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TextInfo
{
    public float value;
    public Transform parent;
    public Enums.FloatingFontType fontType;
}
public class FloatingTextController : MonoBehaviour
{
    [SerializeField]
    GameObject FloatingTextPrefab;
    private Queue<TextInfo> _floatingRewardQueue = new Queue<TextInfo>();

    public void OnEnable()
    {
        _floatingRewardQueue.Clear();
        StartCoroutine(CoWaitRewardText());
    }

    public void RegisterOrSpawnText(float value, Transform parent, Enums.FloatingFontType fontType, bool isReward = false)
    {
        if (isReward == true)
        {
            TextInfo info = new TextInfo()
            {
                value = value,
                parent = parent,
                fontType = fontType
            };

            _floatingRewardQueue.Enqueue(info);
            return;
        }

        SpawnFloatingText(value, parent, fontType);
    }

    public void SpawnFloatingText(float value, Transform parent, Enums.FloatingFontType fontType)
    {
        
        GameObject go = Managers.ResourceManager.Instantiate(FloatingTextPrefab.name, parent, isPool:true);
        FloatingText text = go.GetComponent<FloatingText>();

        text.SetInfo(value, fontType);
    }

    IEnumerator CoWaitRewardText()
    {
        WaitForSeconds sec = new WaitForSeconds(0.5f);
        while (true)
        {
            if (_floatingRewardQueue.Count > 0)
            {
                TextInfo info = _floatingRewardQueue.Dequeue();
                SpawnFloatingText(info.value, info.parent, info.fontType);
            }
            yield return sec;
        }
    }
}
