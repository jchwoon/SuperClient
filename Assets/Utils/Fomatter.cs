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
            skillDescBuilder.Replace("{maxEntityCount}", skillData.MaxEntityCount.ToString());
        }
        if (IsValidateOldValue(skillDescBuilder.ToString(), "{addValue}"))
        {
            skillDescBuilder.Replace("{addValue}", FormatSkillEffectAddValues(effect, skillLevel));
        }

        return skillDescBuilder.ToString();
    }

    public static string FormatSkillCost(SkillData skillData)
    {
        return $"MP {skillData.CostMp}�Һ�";
    }
    public static string FormatSkillCool(SkillData skillData)
    {
        return $"���� ���ð� {skillData.CoolTime}��";
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

    private static bool IsValidateOldValue(string text, string oldValue)
    {
        return text.Contains(oldValue);
    }
}
