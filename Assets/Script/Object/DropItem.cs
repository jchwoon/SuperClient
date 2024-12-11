using Data;
using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : BaseObject
{
    public ItemData ItemData { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Update()
    {
        base.Update();
    }

    public void Init(ObjectInfo info)
    {
        ItemData itemData;
        if (Managers.DataManager.ItemDict.TryGetValue(info.TemplateId, out itemData) == false)
            return;

        ItemData = itemData;
        Name = itemData.Name;
        SetPos(gameObject, info.PosInfo);
        SetObjInfo(info);
    }

    public override void OnContactMyHero(MyHero hero)
    {
        if (hero == null)
            return;
        hero.ReqPickupItem(ObjectId);
    }
}
