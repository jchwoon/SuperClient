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
    Dictionary<int, DropItem> _dropItems = new Dictionary<int, DropItem>();
    Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>(); 
    Dictionary<int, NPC> _npcs = new Dictionary<int, NPC>(); 
    public MyHero Spawn(MyHeroInfo myHeroInfo, Action<MyHero> initComp)
    {
        HeroInfo heroInfo = myHeroInfo.HeroInfo;
        ObjectInfo objectInfo = heroInfo.CreatureInfo.ObjectInfo;

        HeroData heroData;
        if (Managers.DataManager.HeroDict.TryGetValue(heroInfo.LobbyHeroInfo.ClassType, out heroData) == false)
            return null;

        GameObject go = Managers.ResourceManager.Instantiate($"{heroInfo.LobbyHeroInfo.ClassType}");
        go.name = "MyHero";
        MyHero myHero = go.AddComponent<MyHero>();
        MyHero = myHero;
        myHero.Init(myHeroInfo, heroData);
        _objects.Add(objectInfo.ObjectId, go);

        initComp?.Invoke(myHero);
        return myHero;
    }

    public Hero Spawn(HeroInfo heroInfo)
    {
        ObjectInfo objectInfo = heroInfo.CreatureInfo.ObjectInfo;

        HeroData heroData;
        if (Managers.DataManager.HeroDict.TryGetValue(heroInfo.LobbyHeroInfo.ClassType, out heroData) == false)
            return null;

        GameObject go = Managers.ResourceManager.Instantiate($"{heroInfo.LobbyHeroInfo.ClassType}");
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
        {
            MonsterSpawn(creatureInfo);
        }
        else if(type == EObjectType.Npc)
        {
            NPCSpawn(creatureInfo);
        }
    }

    public void Spawn(ObjectInfo objectInfo)
    {
        EObjectType type = objectInfo.ObjectType;
        if (type == EObjectType.DropItem)
            DropItemSpawn(objectInfo);
    }
    private Monster MonsterSpawn(CreatureInfo creatureInfo)
    {
        ObjectInfo objectInfo = creatureInfo.ObjectInfo;

        MonsterData monsterData;
        if (Managers.DataManager.MonsterDict.TryGetValue(creatureInfo.ObjectInfo.TemplateId, out monsterData) == false)
            return null;

        GameObject go = Managers.ResourceManager.Instantiate(monsterData.PrefabName, isPool:true);
        go.name = $"{monsterData.Name}";
        Monster monster = Utils.GetOrAddComponent<Monster>(go);
        monster.Init(creatureInfo);
        _objects.Add(objectInfo.ObjectId, go);
        _monsters.Add(objectInfo.ObjectId, monster);

        return monster;
    }

    private DropItem DropItemSpawn(ObjectInfo objectInfo)
    {
        ItemData itemData;
        if (Managers.DataManager.ItemDict.TryGetValue(objectInfo.TemplateId, out itemData) == false)
            return null;

        GameObject go = Managers.ResourceManager.Instantiate(itemData.PrefabName, isPool: false);
        go.name = $"{itemData.Name}";
        DropItem dropItem = Utils.GetOrAddComponent<DropItem>(go);
        dropItem.Init(objectInfo);
        _objects.Add(objectInfo.ObjectId, go);
        _dropItems.Add(objectInfo.ObjectId, dropItem);

        return dropItem;
    }
    private NPC NPCSpawn(CreatureInfo creatureInfo)
    {
        ObjectInfo objectInfo = creatureInfo.ObjectInfo;

        NPCData npcData;
        if (Managers.DataManager.NpcDict.TryGetValue(creatureInfo.ObjectInfo.TemplateId, out npcData) == false)
            return null;

        GameObject go = Managers.ResourceManager.Instantiate(npcData.PrefabName, isPool: true);
        go.name = $"{npcData.Name}";
        NPC npc = Utils.GetOrAddComponent<NPC>(go);
        npc.Init(objectInfo);
        _objects.Add(objectInfo.ObjectId, go);
        _npcs.Add(objectInfo.ObjectId, npc);

        return npc;
    }
    public void DeSpawn(int objectId)
    {
        GameObject go = FindById(objectId);
        if (go == null)
            return;
        _objects.Remove(objectId);
        _heroes.Remove(objectId);
        _monsters.Remove(objectId);
        _dropItems.Remove(objectId);

        //Temp
        BaseObject creature = go.GetComponent<BaseObject>();
        Managers.ResourceManager.Destroy(go, isPool: creature.ObjectType == EObjectType.Monster ? true : false);
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

    public List<DropItem> GetAllDropItem()
    {
        List<DropItem> items = new List<DropItem>();

        foreach (DropItem item in _dropItems.Values)
        {
            items.Add(item);
        }

        return items;
    }

    public void Clear()
    {
        if (MyHero != null)
            MyHero = null;
        
        _objects.Clear();
        _monsters.Clear();
        _heroes.Clear();
        _dropItems.Clear();
    }
}
