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
    ///////////////////////////////
    ///////////STAT////////////////
    ///////////////////////////////
    public class HeroStat
    {
        public int Level;
        public int AD;
        public int MaxHp;
        public int MaxMp;
        public int Defence;
        public float AtkSpeed;
        public float MoveSpeed;
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

