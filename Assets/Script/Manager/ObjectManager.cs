using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager
{
    public MyHero MyHero { get; private set; }
    Dictionary<int, Hero> _heroes = new Dictionary<int, Hero>(); 
    public MyHero Spawn(MyHeroInfo myHeroInfo)
    {
        HeroInfo heroInfo = myHeroInfo.HeroInfo;

        GameObject go = Managers.ResourceManager.Instantiate($"{heroInfo.LobbyHeroInfo.ClassType}_Init");
        go.name = "MyHero";
        MyHero myHero = go.AddComponent<MyHero>();
        myHero.SetInfo(myHeroInfo);
        MyHero = myHero;

        return myHero;
    }

    public Hero Spawn(HeroInfo heroInfo)
    {
        GameObject go = Managers.ResourceManager.Instantiate($"{heroInfo.LobbyHeroInfo.ClassType}_Init");
        go.name = $"{heroInfo.LobbyHeroInfo.Nickname}";
        Hero hero = go.AddComponent<Hero>();
        hero.SetInfo(heroInfo);

        _heroes.Add(heroInfo.ObjectInfo.ObjectId, hero);

        return hero;
    }
}
