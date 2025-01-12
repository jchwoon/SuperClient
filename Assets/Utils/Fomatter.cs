using Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Fomatter 
{

    public static string FormatSkillDescription(SkillData skillData, DescriptionData desc, int skillLevel, EffectData effect = null)
    {
        var skillDescBuilder = new StringBuilder(desc.DetailText);

        if (IsValidateOldValue(skillDescBuilder.ToString(), "{ratio}"))
        {
            skillDescBuilder.Replace("{ratio}", (effect.Ratio[skillLevel - 1] * 100).ToString());
        }
        if (IsValidateOldValue(skillDescBuilder.ToString(), "{duration}"))
        {
            skillDescBuilder.Replace("{duration}", effect.Duration.ToString());
        }
        if (IsValidateOldValue(skillDescBuilder.ToString(), "{maxEntityCount}"))
        {
            ActiveSkillData activeSkillData = (ActiveSkillData)skillData;
            skillDescBuilder.Replace("{maxEntityCount}", activeSkillData.MaxEntityCount.ToString());
        }
        if (IsValidateOldValue(skillDescBuilder.ToString(), "{addValue}"))
        {
            skillDescBuilder.Replace("{addValue}", FormatSkillEffectAddValues(effect, skillLevel));
        }

        return skillDescBuilder.ToString();
    }

    public static string FormatSkillCost(ActiveSkillData skillData)
    {
        return $"MP {skillData.CostMp}소비";
    }
    public static string FormatSkillCool(ActiveSkillData skillData)
    {
        return $"재사용 대기시간 {skillData.CoolTime}초";
    }

    public static string FormatSkillEffectAddValues(EffectData effect, int skillLevel)
    {
        string ret = "";
        if (effect.AddStatValues != null)
        {
            foreach (var value in effect.AddStatValues)
            {
                ret += $"{value.addValue[skillLevel - 1]}, ";
            }
        }

        return ret;
    }

    public static string FromatCurrency(int value)
    {
        return value.ToString("N0");
    }

    private static bool IsValidateOldValue(string text, string oldValue)
    {
        return text.Contains(oldValue);
    }
}
