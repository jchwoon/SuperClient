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
    public Dictionary<int, HeroStatData> HeroStatDict { get; private set; } = new Dictionary<int, HeroStatData>();
    public Dictionary<int, RoomData> RoomDict { get; private set; } = new Dictionary<int, RoomData>();
    public Dictionary<int, MonsterData> MonsterDict { get; private set; } = new Dictionary<int, MonsterData>();
    public Dictionary<EHeroClassType, HeroData> HeroDict { get; private set; } = new Dictionary<EHeroClassType, HeroData>();
    public Dictionary<int, SkillData> SkillDict { get; private set; } = new Dictionary<int, SkillData>();
    public Dictionary<int, EffectData> EffectDict { get; private set; } = new Dictionary<int, EffectData>();
    public Dictionary<int, RewardData> RewardDict { get; private set; } = new Dictionary<int, RewardData>();
    public Dictionary<int, RewardTableData> RewardTableDict { get; private set; } = new Dictionary<int, RewardTableData>();
    public Dictionary<int, ItemData> ItemDict { get; private set; } = new Dictionary<int, ItemData>();
    public Dictionary<int, ConsumableData> ConsumableDict { get; private set; } = new Dictionary<int, ConsumableData>();

    public  void Init()
    {
        HeroStatDict = LoadJson<HeroStatDataLoader, int, HeroStatData>("HeroStatData").MakeDict();
        HeroDict = LoadJson<HeroDataLoader, EHeroClassType, HeroData>("HeroData").MakeDict();
        RoomDict = LoadJson<RoomDataLoader, int, RoomData>("RoomData").MakeDict();
        MonsterDict = LoadJson<MonsterDataLoader, int, MonsterData>("MonsterData").MakeDict();
        SkillDict = LoadJson<SkillDataLoader, int, SkillData>("SkillData").MakeDict();
        EffectDict = LoadJson<EffectDataLoader, int, EffectData>("EffectData").MakeDict();
        RewardDict = LoadJson<RewardDataLoader, int, RewardData>("RewardData").MakeDict();
        RewardTableDict = LoadJson<RewardTableDataLoadaer, int, RewardTableData>("RewardTableData").MakeDict();
        //Item
        ConsumableDict = LoadJson<ConsumableDataLoader, int, ConsumableData>("ConsumableData").MakeDict();
        MakeItemDict();
    }

    private  Loader LoadJson<Loader, Key, Value>(string key) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.ResourceManager.GetResource<TextAsset>(key);

        Loader loader = JsonConvert.DeserializeObject<Loader>(textAsset.text);
        return loader;
    }
    private void MakeItemDict()
    {
        foreach (ConsumableData consumable in ConsumableDict.Values)
        {
            ItemDict.Add(consumable.ItemId, consumable);
        }
    }
}