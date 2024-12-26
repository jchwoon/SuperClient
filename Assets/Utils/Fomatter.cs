using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fomatter 
{
    public static string FormatSkillDescription(SkillData skillData, DescriptionData desc, int skillLevel, EffectData effect = null)
    {
        string skillDesc = "";
        skillDesc += FormatSkillCost(skillData);
        skillDesc += ", ";
        skillDesc += desc.DetailText
            .Replace("{healthRatio}", ((effect.HealthRatio + (effect.GapPerLevel * (skillLevel - 1))) * 100).ToString())
            .Replace("{duration}", effect.Duration.ToString())
            .Replace("{entityRatio}", effect.EntityRatio.ToString("F1"))
            .Replace("{damageRatio}", ((effect.DamageRatio + (effect.GapPerLevel * (skillLevel - 1))) * 100).ToString());
        skillDesc += "\n";
        skillDesc += FormatSkillCool(skillData);

        return skillDesc;
    }

    public static string FormatSkillCost(SkillData skillData)
    {
        return $"MP {skillData.CostMp}소비";
    }
    public static string FormatSkillCool(SkillData skillData)
    {
        return $"재사용 대기시간 {skillData.CoolTime}초";
    }
}
