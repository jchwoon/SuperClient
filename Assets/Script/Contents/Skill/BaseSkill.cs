using Data;
using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSkill
{
    public MyHero Owner { get; protected set; }
    public int TemplateId { get; protected set; }
    public SkillData SkillData { get; protected set; }
    public int CurrentSkillLevel { get; protected set; }


    public BaseSkill(int templateId, MyHero owner, SkillData skillData, int skillLevel)
    {
        TemplateId = templateId;
        Owner = owner;
        SkillData = skillData;
        CurrentSkillLevel = skillLevel;
    }

    public bool CheckCanLevelUp(int point)
    {
        int maxLevel = SkillData.MaxLevel;

        if (maxLevel < CurrentSkillLevel + point)
            return false;

        return true;
    }
    
    public void UpdateSkillLevel(int level)
    {
        CurrentSkillLevel = level;
    }
}

