using Google.Protobuf.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();

        return component;
    }
    public static int GetAccountId()
    {
        //return 5;
        int deviceIdHash = SystemInfo.deviceUniqueIdentifier.GetHashCode();
        return deviceIdHash;
    }
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>(true))
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    public static SkillComponent GetMySkillComponent()
    {
        MyHero hero = Managers.ObjectManager.MyHero;
        if (hero == null)
            return null;

        return hero.SkillComponent;
    }

    private static readonly Dictionary<EStatType, string> _statToTextMap = new Dictionary<EStatType, string>
    {
        { EStatType.MaxHp, "�ִ� ü��" },
        { EStatType.MaxMp, "�ִ� ����" },
        { EStatType.Hp, "ü��" },
        { EStatType.Mp, "����" },
        { EStatType.Atk, "���ݷ�" },
        { EStatType.Defence, "����" },
        { EStatType.MoveSpeed, "�̵��ӵ�" },
    };

    public static string GetStatTypeText(EStatType statType)
    {
        if (_statToTextMap.TryGetValue(statType, out string text) == true)
            return text;
        return string.Empty;
    }

    private static readonly Dictionary<EHeroClassType, string> _classToTextMap = new Dictionary<EHeroClassType, string>
    {
        { EHeroClassType.Guardian, "�����" },
        { EHeroClassType.Wizard, "���ڵ�" },
        { EHeroClassType.Priest, "������Ʈ" },
    };

    public static string GetClassTypeText(EHeroClassType classType)
    {
        if (_classToTextMap.TryGetValue(classType, out string text) == true)
            return text;
        return string.Empty;
    }

    public static float GetAngleFromDir(Vector2 dir)
    {
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    public static long TickCount { get { return (long)(Time.time * 1000); } }
}
