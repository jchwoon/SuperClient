using Data;
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
        Stat = new StatComponent(this);
        Stat.InitStat(info.StatInfo);
        SetPos(gameObject, info.ObjectInfo.PosInfo);
        SetObjInfo(info);
    }
}
