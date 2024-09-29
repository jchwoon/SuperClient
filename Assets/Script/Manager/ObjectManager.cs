using Data;
using Google.Protobuf.Enum;
using Google.Protobuf.Struct;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectManager
{
    public MyHero MyHero { get; private set; }
    Dictionary<int, Hero> _heroes = new Dictionary<int, Hero>(); 
    Dictionary<int, Monster> _monsters = new Dictionary<int, Monster>(); 
    Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>(); 
    public MyHero Spawn(MyHeroInfo myHeroInfo)
    {
        HeroInfo heroInfo = myHeroInfo.HeroInfo;
        ObjectInfo objectInfo = heroInfo.CreatureInfo.ObjectInfo;

        GameObject go = Managers.ResourceManager.Instantiate($"{heroInfo.LobbyHeroInfo.ClassType}_Init");
        go.name = "MyHero";
        SetPos(go, objectInfo.PosInfo);
        MyHero myHero = go.AddComponent<MyHero>();
        myHero.SetInfo(myHeroInfo);
        _objects.Add(objectInfo.ObjectId, go);
        MyHero = myHero;
        return myHero;
    }

    public Hero Spawn(HeroInfo heroInfo)
    {
        ObjectInfo objectInfo = heroInfo.CreatureInfo.ObjectInfo;

        GameObject go = Managers.ResourceManager.Instantiate($"{heroInfo.LobbyHeroInfo.ClassType}_Init");
        go.name = $"{heroInfo.LobbyHeroInfo.Nickname}";
        SetPos(go, objectInfo.PosInfo);
        Hero hero = go.AddComponent<Hero>();
        hero.SetInfo(heroInfo);
        _objects.Add(objectInfo.ObjectId, go);
        _heroes.Add(objectInfo.ObjectId, hero);

        return hero;
    }

    public void Spawn(CreatureInfo creatureInfo)
    {
        EObjectType type = creatureInfo.ObjectInfo.ObjectType;
        if (type == EObjectType.Monster)
            MonsterSpawn(creatureInfo);
    }
    private Monster MonsterSpawn(CreatureInfo creatureInfo)
    {
        ObjectInfo objectInfo = creatureInfo.ObjectInfo;

        MonsterData monsterData;
        if (Managers.DataManager.MonsterDict.TryGetValue(creatureInfo.ObjectInfo.TemplateId, out monsterData) == false)
            return null;

        GameObject go = Managers.ResourceManager.Instantiate(monsterData.PrefabName);
        go.name = $"{monsterData.Name}";
        SetPos(go, objectInfo.PosInfo);
        Monster monster = go.AddComponent<Monster>();
        monster.SetInfo(creatureInfo);
        _objects.Add(objectInfo.ObjectId, go);
        _monsters.Add(objectInfo.ObjectId, monster);

        return monster;
    }
    public void DeSpawn(int objectId, EObjectType objType)
    {
        //MyHero를 삭제할 일이 있을 경우 만들어주기 Todo
        GameObject go = FindById(objectId);
        if (go == null)
            return;
        _objects.Remove(objectId);
        _heroes.Remove(objectId);

        Managers.ResourceManager.Destroy(go);
    }

    public GameObject FindById(int objectId)
    {
        GameObject go = null;
        _objects.TryGetValue(objectId, out go);
        return go;
    }
    private void SetPos(GameObject go, PosInfo posInfo)
    {
        go.transform.position = new Vector3(posInfo.PosX, posInfo.PosY, posInfo.PosZ);
        go.transform.eulerAngles = new Vector3(0, posInfo.RotY, 0);
    }
}
