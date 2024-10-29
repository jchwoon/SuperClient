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
    public class HeroStatData
    {
        public int Level;
        public int Exp;
        public int MaxHp;
        public int MaxMp;
        public float MoveSpeed;
        public int AtkDamage;
        public int Defence;
        public float AtkSpeed;
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

    public class SkillData
    {
        public int SkillId;
        public ESkillType SkillType;
        public string SkillName;
        public string AnimName;
        public float SkillRange;
        public int CostMp;
        public float CoolTime;
        public float AnimTime;
        public string AnimParamName;
        public float ComboTime;
        public int MaxComboIdx;
        public List<string> ComboNames;
    }

    [Serializable]
    public class SkillDataLoader : ILoader<int, SkillData>
    {
        public List<SkillData> skills = new List<SkillData>();
        public Dictionary<int, SkillData> MakeDict()
        {
            Dictionary<int, SkillData> dict = new Dictionary<int, SkillData>();
            foreach(SkillData skill in skills)
            {
                dict.Add(skill.SkillId, skill);
            }

            return dict;
        }
    }

    public struct AddStatInfo
    {
        public EStatType StatType;
        public float Value;
    }

    public class EffectData
    {
        public int EffectId;
        public float DamageRatio;
        public List<AddStatInfo> AddStatValues;
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
                dict.Add(effect.EffectId, effect);
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
        public bool Stackable;
        public int MaxStack;
        public string Name;
    }

    public class ConsumableData : ItemData
    {
        public int EffectId;
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
}

