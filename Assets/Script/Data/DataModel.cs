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
    public class HeroStat
    {
        public int Level;
        public int AD;
        public int MaxHp;
        public int MaxMp;
        public int Defence;
        public int AtkSpeed;
        public int MoveSpeed;
        public int Exp;
    }

    [Serializable]
    public class HeroStatDataLoader : ILoader<int, HeroStat>
    {
        public List<HeroStat> heroStats = new List<HeroStat>();

        public Dictionary<int, HeroStat> MakeDict()
        {
            Dictionary<int, HeroStat> dict = new Dictionary<int, HeroStat>();
            foreach (HeroStat stat in heroStats)
                dict.Add(stat.Level, stat);

            return dict;
        }
    }

    public class HeroData
    {
        public EHeroClassType ClassType;
        public HeroStat HeroStat;

    }
}

