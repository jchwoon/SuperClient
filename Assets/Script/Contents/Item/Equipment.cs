using Data;
using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{
    public EquipmentData EquipmentData { get; private set; }
    public Equipment(int itemId) : base(itemId)
    {
        EquipmentData = (EquipmentData)ItemData;
    }

    public bool CheckEquip()
    {
        MyHero myHero = Managers.ObjectManager.MyHero;
        if (myHero == null)
            return false;

        //�ش� �������� Ŭ������ ��ġ�ϴ���
        if (EquipmentData.ClassType != myHero.HeroInfo.LobbyHeroInfo.ClassType)
            return false;
        //�䱸 �������� �� ������
        if (EquipmentData.RequiredLevel > myHero.GrowthInfo.Level)
            return false;

        return true;
    }

    public void SendEquipItem()
    {
        EquipItemToS equipItemPacket = new EquipItemToS();
        equipItemPacket.ItemDbId = ItemDbId;
        Managers.NetworkManager.Send(equipItemPacket);
    }

    public void SendUnEquipItem()
    {
        UnEquipItemToS unEquipItemPacket = new UnEquipItemToS();
        unEquipItemPacket.ItemDbId = ItemDbId;
        Managers.NetworkManager.Send(unEquipItemPacket);
    }
}
