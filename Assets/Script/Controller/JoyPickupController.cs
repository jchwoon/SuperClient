using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyPickupController : MonoBehaviour
{
    public void OnHandlePickupClick()
    {
        MyHero hero = Managers.ObjectManager.MyHero;
        if (hero == null)
            return;

        List<DropItem> items = Managers.ObjectManager.GetAllDropItem();

        if (items.Count <= 0)
        {
            Managers.UIManager.ShowToasUI("획득할 수 있는 아이템이 없습니다.");
            return;
        }

        float closestDist = float.MaxValue;
        int? closestItemId = null;
        foreach (DropItem item in items)
        {
            float dist = (item.transform.position - hero.transform.position).sqrMagnitude;
            if (dist < closestDist)
            {
                closestDist = dist;
                closestItemId = item.ObjectId;
            }
        }
        if (closestItemId.HasValue)
            hero.ReqCheckCanPickup(closestItemId.Value);
    }
}
