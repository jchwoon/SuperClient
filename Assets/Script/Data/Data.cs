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
        public int SkillRange;
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

    ///////////////////////////////
    ///////////TEXT////////////////
    ///////////////////////////////
    //public class TextData
    //{
    //    public string TemplateId;
    //    public string Ko;
    //    public string En;
    //}

    //[Serializable]
    //public class TextDataLoader : ILoader<string, TextData>
    //{
    //    public List<TextData> heroStats = new List<TextData>();

    //    public Dictionary<string, TextData> MakeDict()
    //    {
    //        Dictionary<string, TextData> dict = new Dictionary<string, TextData>();
    //        foreach (TextData text in heroStats)
    //            dict.Add(text.TemplateId, text);

    //        return dict;
    //    }
    //}
}

