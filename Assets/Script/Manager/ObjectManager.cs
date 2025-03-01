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
            SpawnMonster(creatureInfo);
        }
        else if(type == EObjectType.Npc)
        {
            SpawnNPC(creatureInfo);
        }
    }

    public void Spawn(ObjectInfo objectInfo)
    {
        EObjectType type = objectInfo.ObjectType;
        if (type == EObjectType.DropItem)
            SpawnDropItem(objectInfo);
    }
    private Monster SpawnMonster(CreatureInfo creatureInfo)
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

    private DropItem SpawnDropItem(ObjectInfo objectInfo)
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
    private NPC SpawnNPC(CreatureInfo creatureInfo)
    {
        ObjectInfo objectInfo = creatureInfo.ObjectInfo;

        NPCData npcData;
        if (Managers.DataManager.NpcDict.TryGetValue(creatureInfo.ObjectInfo.TemplateId, out npcData) == false)
            return null;

        GameObject go = Managers.ResourceManager.Instantiate(npcData.PrefabName);
        go.name = $"{npcData.Name}";
        NPC npc = null;
        switch (npcData.NpcType)
        {
            case ENPCType.Store:
                npc = Utils.GetOrAddComponent<StoreNPC>(go);
                break;
            case ENPCType.Quest:
                break;
        }
        npc.Init(objectInfo);
        _objects.Add(objectInfo.ObjectId, go);
        _npcs.Add(objectInfo.ObjectId, npc);

        return npc;
    }

    public ParticleController SpawnParticle(ParticleInfo info)
    {
        GameObject go = Managers.ResourceManager.Instantiate(info.PrefabName, info.Parent, isPool : true);
        ParticleController particleController = Utils.GetOrAddComponent<ParticleController>(go);
        particleController.SetInfo(info);
        return particleController;
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
        _npcs.Remove(objectId);

        //Temp
        BaseObject creature = go.GetComponent<BaseObject>();
        Managers.ResourceManager.Destroy(go, isPool: creature.ObjectType == EObjectType.Monster ? true : false);
    }

    public GameObject FindById(int objectId)
    {
        GameObject go = null;
        if (MyHero && MyHero.ObjectId == objectId)
            return MyHero.gameObject;
        _objects.TryGetValue(objectId, out go);
        return go;
    }

    public List<Creature> GetAllCreatures(bool includeMyHero = false)
    {
        List<Creature> creatures = new List<Creature>();

        creatures.AddRange(GetAllMonsters());
        creatures.AddRange(GetAllHeroes(includeMyHero));

        return creatures;
    }

    public List<Monster> GetAllMonsters()
    {
        List<Monster> monsters = new List<Monster>();

        foreach (Monster monster in _monsters.Values)
        {
            monsters.Add(monster);
        }
        return monsters;
    }

    public List<Hero> GetAllHeroes(bool includeMyHero = false)
    {
        List<Hero> heroes = new List<Hero>();

        foreach (Hero hero in _heroes.Values)
        {
            heroes.Add(hero);
        }
        if (includeMyHero)
        {
            heroes.Add(MyHero);
        }
        return heroes;
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

    public void Clear(bool leaveHero = false)
    {
        foreach (GameObject obj in _objects.Values)
            Managers.ResourceManager.Destroy(obj);

        _objects.Clear();
        _monsters.Clear();
        _heroes.Clear();
        _dropItems.Clear();
        _npcs.Clear();
        if (MyHero && leaveHero == false)
        {
            Managers.ResourceManager.Destroy(MyHero.gameObject);
            MyHero = null;
        }
    }
}
