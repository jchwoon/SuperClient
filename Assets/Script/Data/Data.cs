using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data
{
    public class ConfigData
    {
        public Enums.EConfigIds ConfigId;
        public string Name;
        public string Ip;
        public int Port;
    }

    [Serializable]
    public class ConfigDataLoader : ILoader<Enums.EConfigIds, ConfigData>
    {
        public List<ConfigData> configs = new List<ConfigData>();
        public Dictionary<Enums.EConfigIds, ConfigData> MakeDict()
        {
            Dictionary<Enums.EConfigIds, ConfigData> dict = new Dictionary<Enums.EConfigIds, ConfigData>();

            foreach (ConfigData config in configs)
            {
                dict.Add(config.ConfigId, config);
            }

            return dict;
        }
    }
    public class HeroStatData
    {
        public int Level;
        public long Exp;
        public int MaxHp;
        public int MaxMp;
        public float MoveSpeed;
        public int AtkDamage;
        public int Defence;
    }

    [Serializable]
    public class HeroStatDataLoader : ILoader<int, HeroStatData>
    {
        public List<HeroStatData> stats = new List<HeroStatData>();

        public Dictionary<int, HeroStatData> MakeDict()
        {
            Dictionary<int, HeroStatData> dict = new Dictionary<int, HeroStatData>();
            foreach (HeroStatData stat in stats)
            {
                dict.Add(stat.Level, stat);
            }


            return dict;
        }
    }

    public class RoomData
    {
        public int RoomId;
        public string Name;
        public string MapName;
        public float StartPosX;
        public float StartPosY;
        public float StartPosZ;
        public List<int> Npcs;
        public ERoomType RoomType;
        public EDungeonType DungeonType;
        public int MinRequiredLevel;
        public int MaxRequiredLevel;
    }

    [Serializable]
    public class RoomDataLoader : ILoader<int, RoomData>
    {
        public List<RoomData> rooms = new List<RoomData>();

        public Dictionary<int, RoomData> MakeDict()
        {
            Dictionary<int, RoomData> dict = new Dictionary<int, RoomData>();
            foreach (RoomData room in rooms)
            {
                dict.Add(room.RoomId, room);
            }


            return dict;
        }
    }
    public class BaseData
    {
        public string PrefabName;
    }

    public class MonsterData : BaseData
    {
        public int MonsterId;
        public int RoomId;
        public string Name;
        public int Level;
        public int Exp;
        public int Gold;
        public int MaxHp;
        public int MaxMp;
        public float MoveSpeed;
        public float ChaseSpeed;
        public int AtkDamage;
        public int Defence;
        public float AtkSpeed;
        public float Sight;
        public List<int> SkillIds;
        public EMonsterAggroType AggroType;
        public EMonsterGrade MonsterGrade;
    }

    [Serializable]
    public class MonsterDataLoader : ILoader<int, MonsterData>
    {
        public List<MonsterData> monsters = new List<MonsterData>();

        public Dictionary<int, MonsterData> MakeDict()
        {
            Dictionary<int, MonsterData> dict = new Dictionary<int, MonsterData>();
            foreach (MonsterData monster in monsters)
            {
                dict.Add(monster.MonsterId, monster);
            }

            return dict;
        }
    }
    public class HeroData : BaseData
    {
        public EHeroClassType HeroClassId;
        public List<int> SkillIds;
        public float RespawnTime;
        public int SkillInitId;
    }
    [Serializable]
    public class HeroDataLoader : ILoader<EHeroClassType, HeroData>
    {
        public List<HeroData> heroes = new List<HeroData>();
        public Dictionary<EHeroClassType, HeroData> MakeDict()
        {
            Dictionary<EHeroClassType, HeroData> dict = new Dictionary<EHeroClassType, HeroData>();
            foreach (HeroData hero in heroes)
            {
                dict.Add(hero.HeroClassId, hero);
            }

            return dict;
        }
    }

    public class SkillData : BaseData
    {
        public int TemplateId;
        public ESkillType SkillType;
        public ESkillSlotType SkillSlotType;
        public EHeroClassType ClassType;
        public string SkillName;
        public string IconName;
        public int MaxLevel;
        public List<int> EffectIds;
    }

    public class ActiveSkillData : SkillData
    {
        public int NextSkillTemplateId;
        public ESkillProjectileType SkillProjectileType;
        public ESkillAreaType SkillAreaType;
        public ESkillUsageTargetType SkillUsageTargetType;
        public ESkillTargetingType SkillTargetingType;
        public float SectorAngle;
        public bool CanCancel;
        public bool IsComboSkill;
        public bool IsMoveSkill;
        public string SoundLabel;
        public string HitPrefabName;
        public string AnimName;
        public float SkillRange;
        public int CostMp;
        public int MaxEntityCount;
        public float CoolTime;
        public float Speed;
        public float Dist;
        public float AnimTime;
        public float EffectDelayRatio;
        public float ComboTime;
        public float Duration;
    }

    [Serializable]
    public class ActiveSkillDataLoader : ILoader<int, ActiveSkillData>
    {
        public List<ActiveSkillData> skills = new List<ActiveSkillData>();

        public Dictionary<int, ActiveSkillData> MakeDict()
        {
            Dictionary<int, ActiveSkillData> dict = new Dictionary<int, ActiveSkillData>();

            foreach (ActiveSkillData skill in skills)
            {
                dict.Add(skill.TemplateId, skill);
            }

            return dict;
        }
    }

    public class PassiveSkillData : SkillData
    {

    }

    [Serializable]
    public class PassiveSkillDataLoader : ILoader<int, PassiveSkillData>
    {
        public List<PassiveSkillData> skills = new List<PassiveSkillData>();

        public Dictionary<int, PassiveSkillData> MakeDict()
        {
            Dictionary<int, PassiveSkillData> dict = new Dictionary<int, PassiveSkillData>();

            foreach (PassiveSkillData skill in skills)
            {
                dict.Add(skill.TemplateId, skill);
            }

            return dict;
        }
    }

    public struct AddStatInfo
    {
        public EStatType StatType;
        public List<float> addValue;
    }

    public class EffectData : BaseData
    {
        public int TemplateId;
        public List<float> Ratio;
        public float Duration;
        public bool Stackable;
        public bool FeedbackEffect;
        public string SoundLabel;
        public string DescId;
        public List<AddStatInfo> AddStatValues;
        public EEffectType EffectType;
        public EEffectDurationType EffectDurationType;
        public EEffectScalingType EffectScalingType;
    }

    [Serializable]
    public class EffectDataLoader : ILoader<int, EffectData>
    {
        public List<EffectData> effects = new List<EffectData>();
        public Dictionary<int, EffectData> MakeDict()
        {
            Dictionary<int, EffectData> dict = new Dictionary<int, EffectData>();

            foreach (EffectData effect in effects)
            {
                dict.Add(effect.TemplateId, effect);
            }

            return dict;
        }
    }

    public class RewardData
    {
        public int RewardId;
        public int ItemId;
    }

    [Serializable]
    public class RewardDataLoader : ILoader<int, RewardData>
    {
        public List<RewardData> rewards = new List<RewardData>();
        public Dictionary<int, RewardData> MakeDict()
        {
            Dictionary<int, RewardData> dict = new Dictionary<int, RewardData>();

            foreach (RewardData reward in rewards)
            {
                dict.Add(reward.RewardId, reward);
            }

            return dict;
        }
    }

    public struct RewardInfo
    {
        public int RewardId;
        public float Probability;
    }
    public class RewardTableData
    {
        public int RewardTableId;
        public int MonsterId;
        public int RewardGold;
        public int RewardExp;
        public List<RewardInfo> RewardInfos;
    }

    [Serializable]
    public class RewardTableDataLoadaer : ILoader<int, RewardTableData>
    {
        public List<RewardTableData> rewardTables = new List<RewardTableData>();
        public Dictionary<int, RewardTableData> MakeDict()
        {
            Dictionary<int, RewardTableData> dict = new Dictionary<int, RewardTableData>();

            foreach (RewardTableData rewardTableData in rewardTables)
            {
                dict.Add(rewardTableData.RewardTableId, rewardTableData);
            }

            return dict;
        }
    }

    public class ItemData : BaseData
    {
        public int ItemId;
        public int EffectId;
        public string DescId;
        public bool Stackable;
        public int MaxStack;
        public string Name;
        public string ImageName;
        public EItemType ItemType;
    }

    public class ConsumableData : ItemData
    {
        public float CoolTime;
        public EConsumableType ConsumableType;
    }

    [Serializable]
    public class ConsumableDataLoader : ILoader<int, ConsumableData>
    {
        public List<ConsumableData> items = new List<ConsumableData>();

        public Dictionary<int, ConsumableData> MakeDict()
        {
            Dictionary<int, ConsumableData> dict = new Dictionary<int, ConsumableData>();

            foreach (ConsumableData consumableData in items)
            {
                dict.Add(consumableData.ItemId, consumableData);
            }

            return dict;
        }
    }

    public class EquipmentData : ItemData
    {
        public EHeroClassType ClassType;
        public EEquipItemType EquipItemType;
        public int RequiredLevel;
    }

    [Serializable]
    public class EquipmentDataLoader : ILoader<int, EquipmentData>
    {
        public List<EquipmentData> items = new List<EquipmentData>();

        public Dictionary<int, EquipmentData> MakeDict()
        {
            Dictionary<int, EquipmentData> dict = new Dictionary<int, EquipmentData>();

            foreach (EquipmentData equipmentData in items)
            {
                dict.Add(equipmentData.ItemId, equipmentData);
            }

            return dict;
        }
    }
    public class EtcData : ItemData
    {
    }

    [Serializable]
    public class EtcDataLoader : ILoader<int, EtcData>
    {
        public List<EtcData> items = new List<EtcData>();

        public Dictionary<int, EtcData> MakeDict()
        {
            Dictionary<int, EtcData> dict = new Dictionary<int, EtcData>();

            foreach (EtcData etcData in items)
            {
                dict.Add(etcData.ItemId, etcData);
            }

            return dict;
        }
    }

    public class DescriptionData
    {
        public string DescId;
        public string Text;
        public string DetailText;
    }

    [Serializable]
    public class DescriptionDataLoader : ILoader<string, DescriptionData>
    {
        public List<DescriptionData> descriptions = new List<DescriptionData>();

        public Dictionary<string, DescriptionData> MakeDict()
        {
            Dictionary<string, DescriptionData> dict = new Dictionary<string, DescriptionData>();

            foreach (DescriptionData descriptionData in descriptions)
            {
                dict.Add(descriptionData.DescId, descriptionData);
            }

            return dict;
        }
    }

    public class NPCData : BaseData
    {
        public ENPCType NpcType;
        public int NpcId;
        public int RoomId;
        public string Name;
        public float XPos;
        public float YPos;
        public float ZPos;
    }

    [Serializable]
    public class NPCDataLoader : ILoader<int, NPCData>
    {
        public List<NPCData> npcs = new List<NPCData>();
        public Dictionary<int, NPCData> MakeDict()
        {
            Dictionary<int, NPCData> dict = new Dictionary<int, NPCData>();

            foreach (NPCData npcDatas in npcs)
            {
                dict.Add(npcDatas.NpcId, npcDatas);
            }

            return dict;
        }
    }
    
    public class DungeonData : BaseData
    {
        public int RoomId;
        public string Name;
        public int minLv;
        public float XPos;
        public float YPos;
        public float ZPos;
    }

    [Serializable]
    public class DungeonDataLoader : ILoader<int, DungeonData>
    {
        public List<DungeonData> dungeons = new List<DungeonData>();
        public Dictionary<int, DungeonData> MakeDict()
        {
            Dictionary<int, DungeonData> dict = new Dictionary<int, DungeonData>();

            foreach (DungeonData dungeonDatas in dungeons)
            {
                dict.Add(dungeonDatas.RoomId, dungeonDatas);
            }

            return dict;
        }
    }

    public class CostData
    {
        public int TemplateId;
        public ECurrencyType CurrencyType;
        public int CostValue;
    }

    [Serializable]
    public class CostDataLoader : ILoader<int, CostData>
    {
        public List<CostData> costs = new List<CostData>();
        public Dictionary<int, CostData> MakeDict()
        {
            Dictionary<int, CostData> dict = new Dictionary<int, CostData>();

            foreach (CostData cost in costs)
            {
                dict.Add(cost.TemplateId, cost);
            }

            return dict;
        }
    }
}

