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
    public MyHero Spawn(MyHeroInfo myHeroInfo)
    {
        HeroInfo heroInfo = myHeroInfo.HeroInfo;
        ObjectInfo objectInfo = heroInfo.ObjectInfo;

        GameObject go = Managers.ResourceManager.Instantiate($"{heroInfo.LobbyHeroInfo.ClassType}_Init");
        go.name = "MyHero";
        SetPos(go, objectInfo.PosInfo);
        MyHero myHero = go.AddComponent<MyHero>();
        myHero.SetInfo(myHeroInfo);
        MyHero = myHero;

        return myHero;
    }

    public Hero Spawn(HeroInfo heroInfo)
    {
        ObjectInfo objectInfo = heroInfo.ObjectInfo;

        GameObject go = Managers.ResourceManager.Instantiate($"{heroInfo.LobbyHeroInfo.ClassType}_Init");
        go.name = $"{heroInfo.LobbyHeroInfo.Nickname}";
        SetPos(go, objectInfo.PosInfo);
        Hero hero = go.AddComponent<Hero>();
        hero.SetInfo(heroInfo);
        Debug.Log(objectInfo.ObjectId);
        _heroes.Add(heroInfo.ObjectInfo.ObjectId, hero);

        return hero;
    }

    public void DeSpawn(int objectId, EObjectType objType)
    {
        //MyHero를 삭제할 일이 있을 경우 만들어주기 Todo

        GameObject go = null;
        switch(objType)
        {
            case EObjectType.Hero:
                go = Find<Hero>(objectId).gameObject;
                _heroes.Remove(objectId); 
                break;
        }
        Debug.Log(go);
        Managers.ResourceManager.Destroy(go);
    }

    public T Find<T>(int objectId) where T : BaseObject
    {
        T obj = null;
        if (typeof(T) == typeof(Hero))
        {
            Hero hero = null;
            if (_heroes.TryGetValue(objectId, out hero) == false)
                return null;
            obj = hero as T;
        }

        return obj;
    }
    private void SetPos(GameObject go, PosInfo posInfo)
    {
        go.transform.position = new Vector3(posInfo.PosX, posInfo.PosY, posInfo.PosZ);
        go.transform.eulerAngles = new Vector3(0, posInfo.RotY, 0);
    }
}
