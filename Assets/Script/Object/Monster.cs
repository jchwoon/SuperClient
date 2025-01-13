using Data;
using Google.Protobuf.Enum;
using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Creature
{
    public MonsterData MonsterData { get; private set; }
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Update()
    {
        base.Update();
    }

    public void Init(CreatureInfo info)
    {
        MonsterData monsterData;
        if (Managers.DataManager.MonsterDict.TryGetValue(info.ObjectInfo.TemplateId, out monsterData) == false)
            return;

        MonsterData = monsterData;
        Name = MonsterData.Name;
        Stat.InitStat(info.StatInfo);
        SetPos(gameObject, info.ObjectInfo.PosInfo);
        SetObjInfo(info.ObjectInfo);
    }

    public override void HandleModifyOneStat(EStatType statType, float changedValue, float gapValue, EFontType fontType)
    {
        base.HandleModifyOneStat(statType, changedValue, gapValue, fontType);
        if (statType == EStatType.Hp && gapValue < 0 && changedValue != 0)
        {
            Machine.OnDamage();
        }
    }
}
