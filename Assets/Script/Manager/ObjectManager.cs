using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager
{
    public MyHero Spawn(MyHeroInfo myHeroInfo)
    {
        HeroInfo heroInfo = myHeroInfo.HeroInfo;

        Debug.Log(heroInfo.LobbyHeroInfo.ClassType.ToString());
        GameObject go = Managers.ResourceManager.Instantiate($"{heroInfo.LobbyHeroInfo.ClassType}_Init");
        go.name = "MyHero";
        MyHero myHero = go.AddComponent<MyHero>();
        myHero.SetInfo(myHeroInfo);

        return myHero;
    }
}
