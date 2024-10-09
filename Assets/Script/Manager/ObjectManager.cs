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

        HeroData heroData;
        if (Managers.DataManager.HeroDict.TryGetValue(heroInfo.LobbyHeroInfo.ClassType, out heroData) == false)
            return null;

        GameObject go = Managers.ResourceManager.Instantiate($"{heroInfo.LobbyHeroInfo.ClassType}_Init");
        go.name = "MyHero";
        MyHero myHero = go.AddComponent<MyHero>();
        MyHero = myHero;
        myHero.Init(myHeroInfo, heroData);
        _objects.Add(objectInfo.ObjectId, go);
        return myHero;
    }

    public Hero Spawn(HeroInfo heroInfo)
    {
        ObjectInfo objectInfo = heroInfo.CreatureInfo.ObjectInfo;

        HeroData heroData;
        if (Managers.DataManager.HeroDict.TryGetValue(heroInfo.LobbyHeroInfo.ClassType, out heroData) == false)
            return null;

        GameObject go = Managers.ResourceManager.Instantiate($"{heroInfo.LobbyHeroInfo.ClassType}_Init");
        go.name = $"{heroInfo.LobbyHeroInfo.Nickname}";
        Hero hero = go.AddComponent<Hero>();
        hero.Init(heroInfo, heroData);
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
        Monster monster = go.AddComponent<Monster>();
        monster.Init(creatureInfo);
        _objects.Add(objectInfo.ObjectId, go);
        _monsters.Add(objectInfo.ObjectId, monster);

        return monster;
    }
    public void DeSpawn(int objectId)
    {
        //MyHero를 삭제할 일이 있을 경우 만들어주기 Todo
        GameObject go = FindById(objectId);
        if (go == null)
            return;
        _objects.Remove(objectId);
        _heroes.Remove(objectId);
        _monsters.Remove(objectId);

        Managers.ResourceManager.Destroy(go);
    }

    public GameObject FindById(int objectId)
    {
        GameObject go = null;
        _objects.TryGetValue(objectId, out go);
        return go;
    }

    public List<Creature> GetAllCreatures()
    {
        List<Creature> creatures = new List<Creature>();

        foreach(Creature creature in _monsters.Values)
        {
            creatures.Add(creature);
        }
        return creatures;
    }

    public void Clear()
    {
        if (MyHero != null)
            MyHero = null;
        
        _objects.Clear();
        _monsters.Clear();
        _heroes.Clear();
    }
}
