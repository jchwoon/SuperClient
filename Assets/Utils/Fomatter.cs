using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fomatter 
{
    public static string FormatSkillDescription(SkillData skill, DescriptionData desc, EffectData effect = null)
    {
        string skillDesc = "";
        skillDesc += FormatSkillCost(skill);
        skillDesc += ", ";
        skillDesc += desc.DetailText
            .Replace("{healthRatio}", (effect.HealthRatio * 100).ToString("F1"))
            .Replace("{duration}", effect.Duration.ToString())
            .Replace("{entityRatio}", effect.EntityRatio.ToString("F1"))
            .Replace("{damageRatio}", (effect.DamageRatio * 100).ToString("F1"));
        skillDesc += "\n";
        skillDesc += FormatSkillCool(skill);

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
