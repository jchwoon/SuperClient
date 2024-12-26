using Data;
using Google.Protobuf.Enum;
using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillComponent
{
    Dictionary<int, BaseSkill> _skills = new Dictionary<int, BaseSkill>();
    public int NormalSkillId { get; private set; }
    public int DashSkillId { get; private set; }
    public bool isUsingSkill { get; set; }

    public void InitSkill(Dictionary<int, int> skills)
    {
        foreach (KeyValuePair<int/*templateId*/, int/*level*/> s in skills) 
        {
            if (Managers.DataManager.SkillDict.TryGetValue(s.Key, out SkillData skillData) == true)
            {
                if (_skills.ContainsKey(s.Key))
                    continue;

                BaseSkill skill = null;
                switch (skillData.SkillProjectileType)
                {
                    case ESkillProjectileType.None:
                        skill = new NonProjectileSkill(s.Key, Managers.ObjectManager.MyHero, skillData, s.Value);
                        break;
                }
                if (skill != null)
                    _skills.Add(s.Key, skill);
                if (skill.SkillData.SkillSlotType == ESkillSlotType.Normal)
                    NormalSkillId = skill.TemplateId;
                else if (skill.SkillData.SkillSlotType == ESkillSlotType.Dash)
                {
                    JoySceneUI joy = Managers.UIManager.ShowSceneUI<JoySceneUI>();
                    joy.SetDashSkill(skillData);
                    DashSkillId = skill.TemplateId;
                }
            }
        }
        
    }

    public bool CheckCanUseSkill(int templatedId)
    {
        BaseSkill skill = GetSkillById(templatedId);
        if (skill == null)
            return false;

        if (skill.SkillData.CanCancel == false && isUsingSkill)
            return false;

        if (skill.CheckCanUseSkill() != ESkillFailReason.None)
            return false;

        return true;
    }

    public BaseSkill GetSkillById(int templateId)
    {
        BaseSkill skill;
        if (_skills.TryGetValue(templateId, out skill) == false)
            return null;

        return skill;
    }

    public int GetSkillLevelById(int templatedId)
    {
        return GetSkillById(templatedId).SkillLevel;
    }

    public List<BaseSkill> GetAllSkill()
    {
        return _skills.Values.ToList();
    }
}
