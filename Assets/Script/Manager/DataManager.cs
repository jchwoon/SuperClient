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
    bool _isInit = false;
    public Dictionary<int, HeroStatData> HeroStatDict { get; private set; } = new Dictionary<int, HeroStatData>();
    public Dictionary<int, RoomData> RoomDict { get; private set; } = new Dictionary<int, RoomData>();
    public Dictionary<int, MonsterData> MonsterDict { get; private set; } = new Dictionary<int, MonsterData>();
    public Dictionary<EHeroClassType, HeroData> HeroDict { get; private set; } = new Dictionary<EHeroClassType, HeroData>();
    public Dictionary<int, SkillData> HeroSkillDict { get; private set; } = new Dictionary<int, SkillData>();
    public Dictionary<int, SkillData> MonsterSkillDict { get; private set; } = new Dictionary<int, SkillData>();
    public Dictionary<int, SkillData> SkillDict { get; private set; } = new Dictionary<int, SkillData>();
    public Dictionary<int, EffectData> EffectDict { get; private set; } = new Dictionary<int, EffectData>();
    public Dictionary<int, RewardData> RewardDict { get; private set; } = new Dictionary<int, RewardData>();
    public Dictionary<int, RewardTableData> RewardTableDict { get; private set; } = new Dictionary<int, RewardTableData>();
    public Dictionary<int, ItemData> ItemDict { get; private set; } = new Dictionary<int, ItemData>();
    public Dictionary<int, ConsumableData> ConsumableDict { get; private set; } = new Dictionary<int, ConsumableData>();
    public Dictionary<string, DescriptionData> DescriptionDict { get; private set; } = new Dictionary<string, DescriptionData>();
    public Dictionary<int, EquipmentData> EquipmentDict { get; private set; } = new Dictionary<int, EquipmentData>();
    public Dictionary<int, EtcData> EtcDict { get; private set; } = new Dictionary<int, EtcData>();
    public Dictionary<Enums.EConfigIds, ConfigData> ConfigDict { get; private set; } = new Dictionary<Enums.EConfigIds, ConfigData>();
    public Dictionary<int, NPCData> NpcDict { get; private set; } = new Dictionary<int, NPCData>();
    public Dictionary<int, DungeonData> DungeonDict { get; private set; } = new Dictionary<int, DungeonData>();


    public  void Init()
    {
        if (_isInit == true)
            return;
        _isInit = true;
        ConfigDict = LoadJson<ConfigDataLoader, Enums.EConfigIds, ConfigData>("ConfigData").MakeDict();
        HeroStatDict = LoadJson<HeroStatDataLoader, int, HeroStatData>("HeroStatData").MakeDict();
        HeroDict = LoadJson<HeroDataLoader, EHeroClassType, HeroData>("HeroData").MakeDict();
        RoomDict = LoadJson<RoomDataLoader, int, RoomData>("RoomData").MakeDict();
        MonsterDict = LoadJson<MonsterDataLoader, int, MonsterData>("MonsterData").MakeDict();
        EffectDict = LoadJson<EffectDataLoader, int, EffectData>("EffectData").MakeDict();
        RewardDict = LoadJson<RewardDataLoader, int, RewardData>("RewardData").MakeDict();
        RewardTableDict = LoadJson<RewardTableDataLoadaer, int, RewardTableData>("RewardTableData").MakeDict();
        DescriptionDict = LoadJson<DescriptionDataLoader, string, DescriptionData>("DescriptionData").MakeDict();
        NpcDict = LoadJson<NPCDataLoader, int, NPCData>("NPCData").MakeDict();
        DungeonDict = LoadJson<DungeonDataLoader, int, DungeonData>("DungeonData").MakeDict();
        

        //Skill
        HeroSkillDict = LoadJson<SkillDataLoader, int, SkillData>("HeroSkillData").MakeDict();
        MonsterSkillDict = LoadJson<SkillDataLoader, int, SkillData>("MonsterSkillData").MakeDict();
        foreach (KeyValuePair<int, SkillData> skill in HeroSkillDict)
            SkillDict.Add(skill.Key, skill.Value);

        foreach (KeyValuePair<int, SkillData> skill in MonsterSkillDict)
            SkillDict.Add(skill.Key, skill.Value);
        //Item
        ConsumableDict = LoadJson<ConsumableDataLoader, int, ConsumableData>("ConsumableData").MakeDict();
        EquipmentDict = LoadJson<EquipmentDataLoader, int, EquipmentData>("EquipmentData").MakeDict();
        EtcDict = LoadJson<EtcDataLoader, int, EtcData>("EtcData").MakeDict();
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
        //Consumable
        foreach (ConsumableData consumable in ConsumableDict.Values)
        {
            ItemDict.Add(consumable.ItemId, consumable);
        }
        //Equipment
        foreach (EquipmentData equipment in EquipmentDict.Values)
        {
            ItemDict.Add(equipment.ItemId, equipment);
        }
        //Etc
        foreach (EtcData etcData in EtcDict.Values)
        {
            ItemDict.Add(etcData.ItemId, etcData);
        }
    }
}