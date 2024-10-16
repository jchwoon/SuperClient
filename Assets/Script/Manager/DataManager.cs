using Data;
using Google.Protobuf.Enum;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    //public Dictionary<int, HeroStat> HeroDict { get; private set; } = new Dictionary<int, HeroStat>();
    public Dictionary<int, RoomData> RoomDict { get; private set; } = new Dictionary<int, RoomData>();
    public Dictionary<int, MonsterData> MonsterDict { get; private set; } = new Dictionary<int, MonsterData>();
    public Dictionary<EHeroClassType, HeroData> HeroDict { get; private set; } = new Dictionary<EHeroClassType, HeroData>();
    //public Dictionary<string, TextData> TextDict { get; private set; } = new Dictionary<string, TextData>();
    public Dictionary<int, SkillData> SkillDict { get; private set; } = new Dictionary<int, SkillData>();

    public  void Init()
    {
        HeroDict = LoadJson<HeroDataLoader, EHeroClassType, HeroData>("HeroData").MakeDict();
        RoomDict = LoadJson<RoomDataLoader, int, RoomData>("RoomData").MakeDict();
        MonsterDict = LoadJson<MonsterDataLoader, int, MonsterData>("MonsterData").MakeDict();
        SkillDict = LoadJson<SkillDataLoader, int, SkillData>("SkillData").MakeDict();
        //TextDict = LoadJson<TextDataLoader, string, TextData>("TextData").MakeDict();
    }

    private  Loader LoadJson<Loader, Key, Value>(string key) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.ResourceManager.GetResource<TextAsset>(key);

        Loader loader = JsonConvert.DeserializeObject<Loader>(textAsset.text);
        return loader;
    }
}